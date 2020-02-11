using Lidgren.Network;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

partial class Send
{
    // Pacotes do cliente
    public enum Packets
    {
        Connect,
        Write_Server_Data,
        Write_Classes,
        Write_Tiles,
        Write_Maps,
        Write_NPCs,
        Write_Items,
        Write_Sprites,
        Request_Server_Data,
        Request_Classes,
        Request_Tiles,
        Request_Map,
        Request_Maps,
        Request_NPCs,
        Request_Items,
        Request_Sprites
    }

    private static void Serialize(NetOutgoingMessage Data, object Element)
    {
        // Serializa os dados
        MemoryStream Stream = new MemoryStream();
        new BinaryFormatter().Serialize(Stream, Element);
        Data.Write(Stream.GetBuffer().Length);
        Data.Write(Stream.GetBuffer());
        Stream.Close();
    }

    public static void Packet(NetOutgoingMessage Data)
    {
        // Envia os dados ao servidor
        Socket.Device.SendMessage(Data, NetDeliveryMethod.ReliableOrdered);
    }

    public static void Connect()
    {
        NetOutgoingMessage Data = Socket.Device.CreateMessage();

        // Envia os dados
        Data.Write((byte)Packets.Connect);
        Data.Write(Login.Objects.txtName.Text);
        Data.Write(Login.Objects.txtPassword.Text);
        Data.Write(true); // Acesso pelo editor
        Packet(Data);
    }

    public static void Request_Server_Data(bool OpenEditor = false)
    {
        NetOutgoingMessage Data = Socket.Device.CreateMessage();

        // Envia os dados
        Data.Write((byte)Packets.Request_Server_Data);
        Data.Write(OpenEditor);
        Packet(Data);
    }

    public static void Request_Classes(bool OpenEditor = false)
    {
        NetOutgoingMessage Data = Socket.Device.CreateMessage();

        // Envia os dados
        Data.Write((byte)Packets.Request_Classes);
        Data.Write(OpenEditor);
        Packet(Data);
    }

    public static void Request_Tiles(bool OpenEditor = false)
    {
        NetOutgoingMessage Data = Socket.Device.CreateMessage();

        // Envia os dados
        Data.Write((byte)Packets.Request_Tiles);
        Data.Write(OpenEditor);
        Packet(Data);
    }

    public static void Request_Map(short Map_Num)
    {
        NetOutgoingMessage Data = Socket.Device.CreateMessage();

        // Envia os dados
        Data.Write((byte)Packets.Request_Map);
        Data.Write(Map_Num);
        Packet(Data);
    }

    public static void Request_Maps(bool OpenEditor = false)
    {
        NetOutgoingMessage Data = Socket.Device.CreateMessage();

        // Envia os dados
        Data.Write((byte)Packets.Request_Maps);
        Data.Write(OpenEditor);
        Packet(Data);
    }

    public static void Request_NPCs(bool OpenEditor = false)
    {
        NetOutgoingMessage Data = Socket.Device.CreateMessage();

        // Envia os dados
        Data.Write((byte)Packets.Request_NPCs);
        Data.Write(OpenEditor);
        Packet(Data);
    }

    public static void Request_Items(bool OpenEditor = false)
    {
        NetOutgoingMessage Data = Socket.Device.CreateMessage();

        // Envia os dados
        Data.Write((byte)Packets.Request_Items);
        Data.Write(OpenEditor);
        Packet(Data);
    }

    public static void Request_Sprites(bool OpenEditor = false)
    {
        NetOutgoingMessage Data = Socket.Device.CreateMessage();

        // Envia os dados
        Data.Write((byte)Packets.Request_Sprites);
        Data.Write(OpenEditor);
        Packet(Data);
    }

    public static void Write_Server_Data()
    {
        NetOutgoingMessage Data = Socket.Device.CreateMessage();

        // Envia os dados
        Data.Write((byte)Packets.Write_Server_Data);
        Data.Write(Lists.Server_Data.Game_Name);
        Data.Write(Lists.Server_Data.Welcome);
        Data.Write(Lists.Server_Data.Port);
        Data.Write(Lists.Server_Data.Max_Players);
        Data.Write(Lists.Server_Data.Max_Characters);
        Packet(Data);
    }

    public static void Write_Classes()
    {
        NetOutgoingMessage Data = Socket.Device.CreateMessage();

        // Envia os dados
        Data.Write((byte)Packets.Write_Classes);
        Serialize(Data, Lists.Class);
        Packet(Data);
    }

    public static void Write_Tiles()
    {
        NetOutgoingMessage Data = Socket.Device.CreateMessage();

        // Envia os dados
        Data.Write((byte)Packets.Write_Tiles);
        Serialize(Data, Lists.Tile);
        Packet(Data);
    }

    public static void Write_Maps()
    {
        NetOutgoingMessage Data = Socket.Device.CreateMessage();

        // Envia os dados
        Data.Write((byte)Packets.Write_Maps);
        Data.Write((short)Lists.Map.Length);
        for (short Index = 1; Index < Lists.Map.Length; Index++)
        {
            Data.Write((short)(Lists.Map[Index].Revision + 1));
            Data.Write(Lists.Map[Index].Name);
            Data.Write(Lists.Map[Index].Width);
            Data.Write(Lists.Map[Index].Height);
            Data.Write(Lists.Map[Index].Moral);
            Data.Write(Lists.Map[Index].Panorama);
            Data.Write(Lists.Map[Index].Music);
            Data.Write(Lists.Map[Index].Color);
            Data.Write(Lists.Map[Index].Weather.Type);
            Data.Write(Lists.Map[Index].Weather.Intensity);
            Data.Write(Lists.Map[Index].Fog.Texture);
            Data.Write(Lists.Map[Index].Fog.Speed_X);
            Data.Write(Lists.Map[Index].Fog.Speed_Y);
            Data.Write(Lists.Map[Index].Fog.Alpha);
            Data.Write(Lists.Map[Index].Light_Global);
            Data.Write(Lists.Map[Index].Lighting);

            // Ligações
            for (short i = 0; i < (short)Globals.Directions.Count; i++)
                Data.Write(Lists.Map[Index].Link[i]);

            // Camadas
            Data.Write((byte)(Lists.Map[Index].Layer.Count - 1));
            for (byte i = 0; i < Lists.Map[Index].Layer.Count; i++)
            {
                Data.Write(Lists.Map[Index].Layer[i].Name);
                Data.Write(Lists.Map[Index].Layer[i].Type);

                // Azulejos
                for (byte x = 0; x <= Lists.Map[Index].Width; x++)
                    for (byte y = 0; y <= Lists.Map[Index].Height; y++)
                    {
                        Data.Write(Lists.Map[Index].Layer[i].Tile[x, y].X);
                        Data.Write(Lists.Map[Index].Layer[i].Tile[x, y].Y);
                        Data.Write(Lists.Map[Index].Layer[i].Tile[x, y].Tile);
                        Data.Write(Lists.Map[Index].Layer[i].Tile[x, y].Auto);
                    }
            }


            // Dados específicos dos azulejos
            for (byte x = 0; x <= Lists.Map[Index].Width; x++)
                for (byte y = 0; y <= Lists.Map[Index].Height; y++)
                {
                    Data.Write(Lists.Map[Index].Tile[x, y].Attribute);
                    Data.Write(Lists.Map[Index].Tile[x, y].Data_1);
                    Data.Write(Lists.Map[Index].Tile[x, y].Data_2);
                    Data.Write(Lists.Map[Index].Tile[x, y].Data_3);
                    Data.Write(Lists.Map[Index].Tile[x, y].Data_4);
                    Data.Write(Lists.Map[Index].Tile[x, y].Zone);

                    // Bloqueio direcional
                    for (byte i = 0; i < (byte)Globals.Directions.Count; i++)
                        Data.Write(Lists.Map[Index].Tile[x, y].Block[i]);
                }

            // Luzes
            Data.Write((byte)Lists.Map[Index].Light.Count);
            for (byte i = 0; i < Lists.Map[Index].Light.Count; i++)
            {
                Data.Write(Lists.Map[Index].Light[i].X);
                Data.Write(Lists.Map[Index].Light[i].Y);
                Data.Write(Lists.Map[Index].Light[i].Width);
                Data.Write(Lists.Map[Index].Light[i].Height);
            }

            // NPCs
            Data.Write((byte)Lists.Map[Index].NPC.Count);
            for (byte i = 0; i < Lists.Map[Index].NPC.Count; i++)
            {
                Data.Write(Lists.Map[Index].NPC[i].Index);
                Data.Write(Lists.Map[Index].NPC[i].Zone);
                Data.Write(Lists.Map[Index].NPC[i].Spawn);
                Data.Write(Lists.Map[Index].NPC[i].X);
                Data.Write(Lists.Map[Index].NPC[i].Y);
            }
        }
        Packet(Data);
    }

    public static void Write_NPCs()
    {
        NetOutgoingMessage Data = Socket.Device.CreateMessage();

        // Envia os dados
        Data.Write((byte)Packets.Write_NPCs);
        Data.Write((short)Lists.NPC.Length);
        for (short Index = 1; Index < Lists.NPC.Length; Index++)
        {
            Data.Write(Lists.NPC[Index].Name);
            Data.Write(Lists.NPC[Index].SayMsg);
            Data.Write(Lists.NPC[Index].Texture);
            Data.Write(Lists.NPC[Index].Behaviour);
            Data.Write(Lists.NPC[Index].SpawnTime);
            Data.Write(Lists.NPC[Index].Sight);
            Data.Write(Lists.NPC[Index].Experience);
            for (byte i = 0; i < (byte)Globals.Vitals.Count; i++) Data.Write(Lists.NPC[Index].Vital[i]);
            for (byte i = 0; i < (byte)Globals.Attributes.Count; i++) Data.Write(Lists.NPC[Index].Attribute[i]);
            Data.Write((byte)Lists.NPC[Index].Drop.Count);
            for (byte i = 0; i < Lists.NPC[Index].Drop.Count; i++)
            {
                Data.Write(Lists.NPC[Index].Drop[i].Item_Num);
                Data.Write(Lists.NPC[Index].Drop[i].Amount);
                Data.Write(Lists.NPC[Index].Drop[i].Chance);
            }
            Data.Write(Lists.NPC[Index].AttackNPC);
            Data.Write((byte)Lists.NPC[Index].Allie.Count);
            for (byte i = 0; i < Lists.NPC[Index].Allie.Count; i++) Data.Write(Lists.NPC[Index].Allie[i]);
            Data.Write((byte)Lists.NPC[Index].Movement);
            Data.Write(Lists.NPC[Index].Flee_Helth);
        }
        Packet(Data);
    }

    public static void Write_Items()
    {
        NetOutgoingMessage Data = Socket.Device.CreateMessage();

        // Envia os dados
        Data.Write((byte)Packets.Write_Items);
        Serialize(Data, Lists.Item);
        Packet(Data);
    }

    public static void Write_Sprites()
    {
        NetOutgoingMessage Data = Socket.Device.CreateMessage();

        // Envia os dados
        Data.Write((byte)Packets.Write_Sprites);
        Serialize(Data, Lists.Sprite);
        Packet(Data);
    }
}