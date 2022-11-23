using System.Net;
using System.Net.Sockets;

ServerObject server = new(); // Initializing server
await server.ListenAsync(); // Server launching

#region Server configuration
class ServerObject
{
    TcpListener tcpListener = new(IPAddress.Any, 8888); // Server listener
    List<ClientObject> clients = new(); // All connections

    protected internal void RemoveConnection(string id)
    {
        // Getting closed connection by Id
        ClientObject? client = clients.FirstOrDefault(c => c.Id == id);

        // And deleting it from connections list
        if (client != null)
        {
            clients.Remove(client);
        }

        client?.Close();
    }

    // Listening to incoming connections
    protected internal async Task ListenAsync()
    {
        try
        {
            tcpListener.Start();
            Console.WriteLine("Сервер запущен. Ожидание подключений...");

            while (true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();

                ClientObject clientObject = new(tcpClient, this);
                clients.Add(clientObject);
                Task.Run(clientObject.ProcessAsync);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            Disconnect();
        }
    }

    // Message translation to all connected users
    protected internal async Task BroadcastMessageAsync(string message, string id)
    {
        foreach (var client in clients)
        {
            if (client.Id != id) // if client's Id isn't equal to sender's id
            {
                await client.Writer.WriteLineAsync(message); // data sending
                await client.Writer.FlushAsync();
            }
        }
    }

    // Diconnects all users
    protected internal void Disconnect()
    {
        foreach (var client in clients)
        {
            client.Close(); // User disconnection
        }

        tcpListener.Stop(); // Server stop
    }
}
#endregion

#region Client configuration
class ClientObject
{
    protected internal string Id { get; } = Guid.NewGuid().ToString();
    protected internal StreamWriter Writer { get; }
    protected internal StreamReader Reader { get; }

    TcpClient client;
    ServerObject server; // объект сервера

    public ClientObject(TcpClient tcpClient, ServerObject serverObject)
    {
        client = tcpClient;
        server = serverObject;
        // получаем NetworkStream для взаимодействия с сервером
        var stream = client.GetStream();
        // создаем StreamReader для чтения данных
        Reader = new StreamReader(stream);
        // создаем StreamWriter для отправки данных
        Writer = new StreamWriter(stream);
    }

    public async Task ProcessAsync()
    {
        try
        {
            // получаем имя пользователя
            string? userName = await Reader.ReadLineAsync();
            string? message = $"{userName} вошел в чат";
            // посылаем сообщение о входе в чат всем подключенным пользователям
            await server.BroadcastMessageAsync(message, Id);
            Console.WriteLine(message);
            // в бесконечном цикле получаем сообщения от клиента
            while (true)
            {
                try
                {
                    message = await Reader.ReadLineAsync();

                    if (message == null)
                    {
                        continue;
                    }

                    message = $"{userName}: {message}";

                    Console.WriteLine(message);

                    await server.BroadcastMessageAsync(message, Id);
                }
                catch
                {
                    message = $"{userName} покинул чат";
                    Console.WriteLine(message);

                    await server.BroadcastMessageAsync(message, Id);

                    break;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            // в случае выхода из цикла закрываем ресурсы
            server.RemoveConnection(Id);
        }
    }

    // закрытие подключения
    protected internal void Close()
    {
        Writer.Close();
        Reader.Close();
        client.Close();
    }
}
#endregion