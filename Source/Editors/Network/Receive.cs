using Lidgren.Network;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

partial class Receive
{
    // Pacotes do servidor
    private enum Packets
    {
        Alert,
        Connect,
        Server_Data,
        Classes,
        Tiles,
        Maps,
        NPCs,
        Items,
        Sprites
    }

    private static object Deserialize(NetIncomingMessage Data)
    {
        // Deserializa os dados
        BinaryFormatter Formater = new BinaryFormatter();
        Formater.Binder = new Program.Binder();
        return Formater.Deserialize(new MemoryStream(Data.ReadBytes(Data.ReadInt32())));
    }

    public static void Handle(NetIncomingMessage Data)
    {
        // Manuseia os dados recebidos
        switch ((Packets)Data.ReadByte())
        {
            case Packets.Alert: Alert(Data); break;
            case Packets.Connect: Connect(); break;
            case Packets.Server_Data: Server_Data(Data); break;
            case Packets.Classes: Classes(Data); break;
            case Packets.Maps: Maps(Data); break;
            case Packets.NPCs: NPCs(Data); break;
            case Packets.Items: Items(Data); break;
            case Packets.Tiles: Tiles(Data); break;
            case Packets.Sprites: Sprites(Data); break;
        }
    }

    private static void Alert(NetIncomingMessage Data)
    {
        // Mostra a mensagem
        MessageBox.Show(Data.ReadString());
    }

    private static void Connect()
    {
        // Abre a janela de edição
        Login.Objects.Visible = false;
        Selection.Objects.Visible = true;
    }

    private static void Server_Data(NetIncomingMessage Data)
    {
        // Lê os dados
        Lists.Server_Data.Game_Name = Data.ReadString();
        Lists.Server_Data.Welcome = Data.ReadString();
        Lists.Server_Data.Port = Data.ReadInt16();
        Lists.Server_Data.Max_Players = Data.ReadByte();
        Lists.Server_Data.Max_Characters = Data.ReadByte();

        // Abre o editor
        if (Data.ReadBoolean()) Editor_Data.Open();
    }

    private static void Classes(NetIncomingMessage Data)
    {
        // Quantidade de classes
        Lists.Class = new Lists.Structures.Class[Data.ReadByte() + 1];

        for (short i = 1; i < Lists.Class.Length; i++)
        {
            // Redimensiona os valores necessários 
            Lists.Class[i] = new Lists.Structures.Class();
            Lists.Class[i].Vital = new short[(byte)Globals.Vitals.Count];
            Lists.Class[i].Attribute = new short[(byte)Globals.Attributes.Count];
            Lists.Class[i].Tex_Male = new System.Collections.Generic.List<short>();
            Lists.Class[i].Tex_Female = new System.Collections.Generic.List<short>();
            Lists.Class[i].Item = new System.Collections.Generic.List<Tuple<short, short>>();

            // Lê os dados
            Lists.Class[i].Name = Data.ReadString();
            Lists.Class[i].Description = Data.ReadString();
            byte Num_Tex_Male = Data.ReadByte();
            for (byte t = 0; t < Num_Tex_Male; t++) Lists.Class[i].Tex_Male.Add(Data.ReadInt16());
            byte Num_Tex_Female = Data.ReadByte();
            for (byte t = 0; t < Num_Tex_Female; t++) Lists.Class[i].Tex_Female.Add(Data.ReadInt16());
            Lists.Class[i].Spawn_Map = Data.ReadInt16();
            Lists.Class[i].Spawn_Direction = Data.ReadByte();
            Lists.Class[i].Spawn_X = Data.ReadByte();
            Lists.Class[i].Spawn_Y = Data.ReadByte();
            for (byte v = 0; v < (byte)Globals.Vitals.Count; v++) Lists.Class[i].Vital[v] = Data.ReadInt16();
            for (byte a = 0; a < (byte)Globals.Attributes.Count; a++) Lists.Class[i].Attribute[a] = Data.ReadInt16();
            byte Num_Items = Data.ReadByte();
            for (byte a = 0; a < Num_Items; a++) Lists.Class[i].Item.Add(new Tuple<short, short>(Data.ReadInt16(), Data.ReadInt16()));
        }

        // Abre o editor
        if (Data.ReadBoolean()) Editor_Classes.Open();
    }

    private static void Maps(NetIncomingMessage Data)
    {
        // Lê os dados e abre o editor
        Lists.Map = (Lists.Structures.Map[])Deserialize(Data);
        if (Data.ReadBoolean()) Editor_Maps.Open();
    }

    private static void NPCs(NetIncomingMessage Data)
    {
        // Quantidade de nocs
        Lists.NPC = new Lists.Structures.NPC[Data.ReadInt16() + 1];

        for (short i = 1; i < Lists.NPC.Length; i++)
        {
            // Redimensiona os valores necessários 
            Lists.NPC[i] = new Lists.Structures.NPC();
            Lists.NPC[i].Vital = new short[(byte)Globals.Vitals.Count];
            Lists.NPC[i].Attribute = new short[(byte)Globals.Attributes.Count];
            Lists.NPC[i].Drop = new System.Collections.Generic.List<Lists.Structures.NPC_Drop>();
            Lists.NPC[i].Allie = new System.Collections.Generic.List<short>();

            // Lê os dados
            Lists.NPC[i].Name = Data.ReadString();
            Lists.NPC[i].SayMsg = Data.ReadString();
            Lists.NPC[i].Texture = Data.ReadInt16();
            Lists.NPC[i].Behaviour = Data.ReadByte();
            for (byte n = 0; n < (byte)Globals.Vitals.Count; n++) Lists.NPC[i].Vital[n] = Data.ReadInt16();
            Lists.NPC[i].SpawnTime = Data.ReadByte();
            Lists.NPC[i].Sight = Data.ReadByte();
            Lists.NPC[i].Experience = Data.ReadInt32();
            for (byte n = 0; n < (byte)Globals.Attributes.Count; n++) Lists.NPC[i].Attribute[n] = Data.ReadInt16();
            byte Num_Drops = Data.ReadByte();
            for (byte n = 0; n < Num_Drops; n++) Lists.NPC[i].Drop.Add(new Lists.Structures.NPC_Drop(Data.ReadInt16(), Data.ReadInt16(), Data.ReadByte()));
            Lists.NPC[i].AttackNPC = Data.ReadBoolean();
            byte Num_Allies = Data.ReadByte();
            for (byte n = 0; n < Num_Allies; n++) Lists.NPC[i].Allie.Add(Data.ReadInt16());
            Lists.NPC[i].Movement = (Globals.NPC_Movements)Data.ReadByte();
            Lists.NPC[i].Flee_Helth = Data.ReadByte();
        }

        // Abre o editor
        if (Data.ReadBoolean()) Editor_NPCs.Open();
    }

    private static void Items(NetIncomingMessage Data)
    {
        // Lê os dados e abre o editor
        Lists.Item = (Lists.Structures.Item[])Deserialize(Data);
        if (Data.ReadBoolean()) Editor_Items.Open();
    }

    private static void Tiles(NetIncomingMessage Data)
    {
        Lists.Tile = new Lists.Structures.Tile[Data.ReadByte()];

        for (byte i = 1; i < Lists.Tile.Length; i++)
        {
            // Dados básicos
            byte Width = Data.ReadByte();
            byte Height = Data.ReadByte();

            // Dados de cada azulejo
            Clear.Tile(i);
            for (byte x = 0; x <= Width; x++)
                for (byte y = 0; y <= Height; y++)
                {
                    // Faz a leitura correta caso alguma textura do azulejo tiver sido redimensionada
                    if (x > Lists.Tile[i].Width || y > Lists.Tile[i].Height)
                    {
                        Data.ReadByte();
                        for (byte d = 0; d < (byte)Globals.Directions.Count; d++) Data.ReadBoolean();
                        continue;
                    }

                    // Atributos
                    Lists.Tile[i].Data[x, y] = new Lists.Structures.Tile_Data();
                    Lists.Tile[i].Data[x, y].Attribute = Data.ReadByte();
                    Lists.Tile[i].Data[x, y].Block = new bool[(byte)Globals.Directions.Count];

                    // Bloqueio direcional
                    for (byte d = 0; d < (byte)Globals.Directions.Count; d++) Lists.Tile[i].Data[x, y].Block[d] = Data.ReadBoolean();
                }
        }

        // Abre o editor
        if (Data.ReadBoolean())
            Editor_Tiles.Open();
    }

    private static void Sprites(NetIncomingMessage Data)
    {
        // Abre o editor
        if (Data.ReadBoolean()) Editor_Sprites.Open();
    }
}