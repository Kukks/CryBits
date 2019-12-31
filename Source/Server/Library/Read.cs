﻿using System;
using System.IO;

partial class Read
{
    public static void All()
    {
        // Carrega todos os dados
        Console.WriteLine("Loading data.");
        Server_Data();
        Console.WriteLine("Loading classes.");
        Classes();
        Console.WriteLine("Loading NPCs.");
        NPCs();
        Console.WriteLine("Loading items.");
        Items();
        Console.WriteLine("Loading maps.");
        Maps();
        Console.WriteLine("Loading tiles.");
        Tiles();
    }

    public static void Server_Data()
    {
        // Limpa os dados
        Clear.Server_Data();

        // Cria o arquivo caso ele não existir
        if (!Directories.Server_Data.Exists)
        {
            Write.Server_Data();
            return;
        }

        // Cria um sistema binário para a manipulação dos dados
        BinaryReader Data = new BinaryReader(Directories.Server_Data.OpenRead());

        // Lê os dados
        Lists.Server_Data.Game_Name = Data.ReadString();
        Lists.Server_Data.Welcome = Data.ReadString();
        Lists.Server_Data.Port = Data.ReadInt16();
        Lists.Server_Data.Max_Players = Data.ReadByte();
        Lists.Server_Data.Max_Characters = Data.ReadByte();
        Lists.Server_Data.Num_Classes = Data.ReadByte();
        Lists.Server_Data.Num_Tiles = Data.ReadByte();
        Lists.Server_Data.Num_Maps = Data.ReadInt16();
        Lists.Server_Data.Num_NPCs = Data.ReadInt16();
        Lists.Server_Data.Num_Items = Data.ReadInt16();

        // Fecha o sistema
        Data.Dispose();
    }

    public static void Player(byte Index, string Name, bool ReadCharacter = true)
    {
        string Directory = Directories.Accounts.FullName + Name + Directories.Format;

        // Cria um arquivo temporário
        BinaryReader Data = new BinaryReader(File.OpenRead(Directory));

        // Carrega os dados e os adiciona ao cache
        Lists.Player[Index].User = Data.ReadString();
        Lists.Player[Index].Password = Data.ReadString();
        Lists.Player[Index].Acess = (Game.Accesses)Data.ReadByte();

        // Dados do personagem
        if (ReadCharacter)
            for (byte i = 1; i <= Lists.Server_Data.Max_Characters; i++)
            {
                Lists.Player[Index].Character[i].Name = Data.ReadString();
                Lists.Player[Index].Character[i].Class = Data.ReadByte();
                Lists.Player[Index].Character[i].Genre = Data.ReadBoolean();
                Lists.Player[Index].Character[i].Level = Data.ReadInt16();
                Lists.Player[Index].Character[i].Experience = Data.ReadInt16();
                Lists.Player[Index].Character[i].Points = Data.ReadByte();
                Lists.Player[Index].Character[i].Map = Data.ReadInt16();
                Lists.Player[Index].Character[i].X = Data.ReadByte();
                Lists.Player[Index].Character[i].Y = Data.ReadByte();
                Lists.Player[Index].Character[i].Direction = (Game.Directions)Data.ReadByte();
                for (byte n = 0; n < (byte)Game.Vitals.Amount; n++) Lists.Player[Index].Character[i].Vital[n] = Data.ReadInt16();
                for (byte n = 0; n < (byte)Game.Attributes.Amount; n++) Lists.Player[Index].Character[i].Attribute[n] = Data.ReadInt16();
                for (byte n = 1; n <= Game.Max_Inventory; n++)
                {
                    Lists.Player[Index].Character[i].Inventory[n].Item_Num = Data.ReadInt16();
                    Lists.Player[Index].Character[i].Inventory[n].Amount = Data.ReadInt16();
                }
                for (byte n = 0; n < (byte)Game.Equipments.Amount; n++) Lists.Player[Index].Character[i].Equipment[n] = Data.ReadInt16();
                for (byte n = 1; n <= Game.Max_Hotbar; n++)
                {
                    Lists.Player[Index].Character[i].Hotbar[n].Type = Data.ReadByte();
                    Lists.Player[Index].Character[i].Hotbar[n].Slot = Data.ReadByte();
                }
            }

        // Descarrega o arquivo
        Data.Dispose();
    }

    public static string Player_Password(string User)
    {
        // Cria um arquivo temporário
        BinaryReader Data = new BinaryReader(File.OpenRead(Directories.Accounts.FullName + User + Directories.Format));

        // Encontra a senha da conta
        Data.ReadString();
        string Password = Data.ReadString();

        // Descarrega o arquivo
        Data.Dispose();

        // Retorna o valor da função
        return Password;
    }

    public static string Characters_Name()
    {
        // Cria o arquivo caso ele não existir
        if (!Directories.Characters.Exists)
        {
            Write.Characters(string.Empty);
            return string.Empty;
        }

        // Cria um arquivo temporário
        StreamReader Data = new StreamReader(Directories.Characters.FullName);

        // Carrega todos os nomes dos personagens
        string Characters = Data.ReadToEnd();

        // Descarrega o arquivo
        Data.Dispose();

        // Retorna o valor de acordo com o que foi carregado
        return Characters;
    }

    public static void Classes()
    {
        Lists.Class = new Lists.Structures.Class[Lists.Server_Data.Num_Classes + 1];

        // Lê os dados
        for (byte i = 1; i < Lists.Class.Length; i++)
        {
            Clear.Class(i);
            Class(i);
        }
    }

    public static void Class(byte Index)
    {
        FileInfo File = new FileInfo(Directories.Classes.FullName + Index + Directories.Format);

        // Cria o arquivo caso ele não existir
        if (!File.Exists)
        {
            Write.Class(Index);
            return;
        }

        // Cria um sistema binário para a manipulação dos dados
        BinaryReader Data = new BinaryReader(File.OpenRead());

        // Lê os dados
        Lists.Class[Index].Name = Data.ReadString();
        Lists.Class[Index].Texture_Male = Data.ReadInt16();
        Lists.Class[Index].Texture_Female = Data.ReadInt16();
        Lists.Class[Index].Spawn_Map = Data.ReadInt16();
        Lists.Class[Index].Spawn_Direction = Data.ReadByte();
        Lists.Class[Index].Spawn_X = Data.ReadByte();
        Lists.Class[Index].Spawn_Y = Data.ReadByte();
        for (byte i = 0; i < (byte)Game.Vitals.Amount; i++) Lists.Class[Index].Vital[i] = Data.ReadInt16();
        for (byte i = 0; i < (byte)Game.Attributes.Amount; i++) Lists.Class[Index].Attribute[i] = Data.ReadInt16();

        // Fecha o sistema
        Data.Dispose();
    }

    public static void Items()
    {
        Lists.Item = new Lists.Structures.Item[Lists.Server_Data.Num_Items + 1];

        // Lê os dados
        for (byte i = 1; i < Lists.Item.Length; i++)
        {
            Clear.Item(i);
            Item(i);
        }
    }

    public static void Item(byte Index)
    {
        FileInfo File = new FileInfo(Directories.Items.FullName + Index + Directories.Format);

        // Cria o arquivo caso ele não existir
        if (!File.Exists)
        {
            Write.Item(Index);
            return;
        }

        // Cria um sistema binário para a manipulação dos dados
        BinaryReader Data = new BinaryReader(File.OpenRead());

        // Lê os dados
        Lists.Item[Index].Name = Data.ReadString();
        Lists.Item[Index].Description = Data.ReadString();
        Lists.Item[Index].Texture = Data.ReadInt16();
        Lists.Item[Index].Type = Data.ReadByte();
        Lists.Item[Index].Price = Data.ReadInt16();
        Lists.Item[Index].Stackable = Data.ReadBoolean();
        Lists.Item[Index].Bind = Data.ReadBoolean();
        Lists.Item[Index].Req_Level = Data.ReadInt16();
        Lists.Item[Index].Req_Class = Data.ReadByte();
        Lists.Item[Index].Potion_Experience = Data.ReadInt16();
        for (byte i = 0; i < (byte)Game.Vitals.Amount; i++) Lists.Item[Index].Potion_Vital[i] = Data.ReadInt16();
        Lists.Item[Index].Equip_Type = Data.ReadByte();
        for (byte i = 0; i < (byte)Game.Attributes.Amount; i++) Lists.Item[Index].Equip_Attribute[i] = Data.ReadInt16();
        Lists.Item[Index].Weapon_Damage = Data.ReadInt16();

        // Fecha o sistema
        Data.Dispose();
    }

    public static void Maps()
    {
        Lists.Map = new Lists.Structures.Map[Lists.Server_Data.Num_Maps + 1];

        // Lê os dados
        for (short i = 1; i < Lists.Map.Length; i++)
        {
            Clear.Map(i);
            Map(i);
        }
    }

    public static void Map(short Index)
    {
        FileInfo File = new FileInfo(Directories.Maps.FullName + Index + Directories.Format);

        // Cria o arquivo caso ele não existir
        if (!File.Exists)
        {
            Write.Map(Index);
            return;
        }

        // Cria um sistema binário para a manipulação dos dados
        BinaryReader Data = new BinaryReader(File.OpenRead());

        // Lê os dados
        Lists.Map[Index].Revision = Data.ReadInt16();
        Lists.Map[Index].Name = Data.ReadString();
        Lists.Map[Index].Width = Data.ReadByte();
        Lists.Map[Index].Height = Data.ReadByte();
        Lists.Map[Index].Moral = Data.ReadByte();
        Lists.Map[Index].Panorama = Data.ReadByte();
        Lists.Map[Index].Music = Data.ReadByte();
        Lists.Map[Index].Color = Data.ReadInt32();
        Lists.Map[Index].Weather.Type = Data.ReadByte();
        Lists.Map[Index].Weather.Intensity = Data.ReadByte();
        Lists.Map[Index].Fog.Texture = Data.ReadByte();
        Lists.Map[Index].Fog.Speed_X = Data.ReadSByte();
        Lists.Map[Index].Fog.Speed_Y = Data.ReadSByte();
        Lists.Map[Index].Fog.Alpha = Data.ReadByte();
        Lists.Map[Index].Light_Global = Data.ReadByte();
        Lists.Map[Index].Lighting = Data.ReadByte();

        // Ligações
        Lists.Map[Index].Link = new short[(short)Game.Directions.Amount];
        for (short i = 0; i < (short)Game.Directions.Amount; i++)
            Lists.Map[Index].Link[i] = Data.ReadInt16();

        // Quantidade de camadas
        byte Num_Layers = Data.ReadByte();
        Lists.Map[Index].Layer = new System.Collections.Generic.List<Lists.Structures.Map_Layer>();

        // Camadas
        for (byte i = 0; i <= Num_Layers; i++)
        {
            // Dados básicos
            Lists.Map[Index].Layer.Add(new Lists.Structures.Map_Layer());
            Lists.Map[Index].Layer[i].Name = Data.ReadString();
            Lists.Map[Index].Layer[i].Type = Data.ReadByte();

            // Redimensiona os azulejos
            Lists.Map[Index].Layer[i].Tile = new Lists.Structures.Map_Tile_Data[Lists.Map[Index].Width + 1, Lists.Map[Index].Height + 1];

            // Azulejos
            for (byte x = 0; x <= Lists.Map[Index].Width; x++)
                for (byte y = 0; y <= Lists.Map[Index].Height; y++)
                {
                    Lists.Map[Index].Layer[i].Tile[x, y].X = Data.ReadByte();
                    Lists.Map[Index].Layer[i].Tile[x, y].Y = Data.ReadByte();
                    Lists.Map[Index].Layer[i].Tile[x, y].Tile = Data.ReadByte();
                    Lists.Map[Index].Layer[i].Tile[x, y].Auto = Data.ReadBoolean();
                }
        }

        // Dados específicos dos azulejos
        Lists.Map[Index].Tile = new Lists.Structures.Map_Tile[Lists.Map[Index].Width + 1, Lists.Map[Index].Height + 1];
        for (byte x = 0; x <= Lists.Map[Index].Width; x++)
            for (byte y = 0; y <= Lists.Map[Index].Height; y++)
            {
                Lists.Map[Index].Tile[x, y].Attribute = Data.ReadByte();
                Lists.Map[Index].Tile[x, y].Data_1 = Data.ReadInt16();
                Lists.Map[Index].Tile[x, y].Data_2 = Data.ReadInt16();
                Lists.Map[Index].Tile[x, y].Data_3 = Data.ReadInt16();
                Lists.Map[Index].Tile[x, y].Data_4 = Data.ReadInt16();
                Lists.Map[Index].Tile[x, y].Zone = Data.ReadByte();
                Lists.Map[Index].Tile[x, y].Block = new bool[(byte)Game.Directions.Amount];

                for (byte i = 0; i < (byte)Game.Directions.Amount; i++)
                    Lists.Map[Index].Tile[x, y].Block[i] = Data.ReadBoolean();
            }

        // Luzes
        Lists.Map[Index].Light = new Lists.Structures.Map_Light[Data.ReadByte()];
        for (byte i = 0; i < Lists.Map[Index].Light.Length; i++)
        {
            Lists.Map[Index].Light[i].X = Data.ReadByte();
            Lists.Map[Index].Light[i].Y = Data.ReadByte();
            Lists.Map[Index].Light[i].Width = Data.ReadByte();
            Lists.Map[Index].Light[i].Height = Data.ReadByte();
        }

        // NPCs
        Lists.Map[Index].NPC = new Lists.Structures.Map_NPC[Data.ReadByte()];
        Lists.Map[Index].Temp_NPC = new Lists.Structures.Map_NPCs[Lists.Map[Index].NPC.Length];
        for (byte i = 0; i < Lists.Map[Index].NPC.Length; i++)
        {
            Lists.Map[Index].NPC[i].Index = Data.ReadInt16();
            Lists.Map[Index].NPC[i].Zone = Data.ReadByte();
            Lists.Map[Index].NPC[i].Spawn = Data.ReadBoolean();
            Lists.Map[Index].NPC[i].X = Data.ReadByte();
            Lists.Map[Index].NPC[i].Y = Data.ReadByte();
        }

        // Items
        Lists.Map[Index].Temp_Item = new System.Collections.Generic.List<Lists.Structures.Map_Items>();
        Lists.Map[Index].Temp_Item.Add(new Lists.Structures.Map_Items()); // Nulo
        global::Map.Spawn_Items(Index);

        // Fecha o sistema
        Data.Dispose();
    }

    public static void NPCs()
    {
        Lists.NPC = new Lists.Structures.NPC[Lists.Server_Data.Num_NPCs + 1];

        // Lê os dados
        for (byte i = 1; i < Lists.NPC.Length; i++)
        {
            Clear.NPC(i);
            NPC(i);
        }
    }

    public static void NPC(byte Index)
    {
        FileInfo File = new FileInfo(Directories.NPCs.FullName + Index + Directories.Format);

        // Cria o arquivo caso ele não existir
        if (!File.Exists)
        {
            Write.NPC(Index);
            return;
        }

        // Cria um sistema binário para a manipulação dos dados
        BinaryReader Data = new BinaryReader(File.OpenRead());

        // Lê os dados
        Lists.NPC[Index].Name = Data.ReadString();
        Lists.NPC[Index].Texture = Data.ReadInt16();
        Lists.NPC[Index].Behaviour = Data.ReadByte();
        Lists.NPC[Index].SpawnTime = Data.ReadByte();
        Lists.NPC[Index].Sight = Data.ReadByte();
        Lists.NPC[Index].Experience = Data.ReadByte();
        for (byte i = 0; i < (byte)Game.Vitals.Amount; i++) Lists.NPC[Index].Vital[i] = Data.ReadInt16();
        for (byte i = 0; i < (byte)Game.Attributes.Amount; i++) Lists.NPC[Index].Attribute[i] = Data.ReadInt16();
        for (byte i = 0; i < Game.Max_NPC_Drop; i++)
        {
            Lists.NPC[Index].Drop[i].Item_Num = Data.ReadInt16();
            Lists.NPC[Index].Drop[i].Amount = Data.ReadInt16();
            Lists.NPC[Index].Drop[i].Chance = Data.ReadByte();
        }

        // Fecha o sistema
        Data.Dispose();
    }

    public static void Tiles()
    {
        Lists.Tile = new Lists.Structures.Tile[Lists.Server_Data.Num_Tiles + 1];

        // Limpa e lê os dados
        for (byte i = 1; i < Lists.Tile.Length; i++)
            Tile(i);
    }

    public static void Tile(byte Index)
    {
        FileInfo File = new FileInfo(Directories.Tiles.FullName + Index + Directories.Format);

        // Evita erros
        if (!File.Exists) return;

        // Cria um sistema binário para a manipulação dos dados
        BinaryReader Data = new BinaryReader(File.OpenRead());

        // Dados básicos
        Lists.Tile[Index].Width = Data.ReadByte();
        Lists.Tile[Index].Height = Data.ReadByte();
        Lists.Tile[Index].Data = new Lists.Structures.Tile_Data[Lists.Tile[Index].Width + 1, Lists.Tile[Index].Height + 1];

        for (byte x = 0; x <= Lists.Tile[Index].Width; x++)
            for (byte y = 0; y <= Lists.Tile[Index].Height; y++)
            {
                // Atributos
                Lists.Tile[Index].Data[x, y].Attribute = Data.ReadByte();

                // Bloqueio direcional
                for (byte i = 0; i < (byte)Game.Directions.Amount; i++){
                    Lists.Tile[Index].Data[x, y].Block = new bool[(byte)Game.Directions.Amount];
                    Lists.Tile[Index].Data[x, y].Block[i] = Data.ReadBoolean();
                }
            }

        // Fecha o sistema
        Data.Dispose();
    }
}