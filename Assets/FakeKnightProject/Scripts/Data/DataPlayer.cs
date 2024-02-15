using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DataGame
{
    public string name;
    public int idPlayer;
    public int lv;
    public int gold;
    public int diamondPurple;
    public int diamondRed;
    public bool isMusic;
    public bool isSound;
    public float speed;
    public int[,] items;
    public int[,] types;
    public int[] equipments;
    // GiftDay
    public int gift;
    public string day;
    public DataGame(string name, int idPlayer, int lv, int gold, int diamondPurple, int diamondRed, bool isMusic, bool isSound, float speed, int[,] items, int[,] types, int[] equipments, int gift, string day)
    {
        this.name = name;
        this.idPlayer = idPlayer;
        this.lv = lv;
        this.gold = gold;
        this.diamondPurple = diamondPurple;
        this.diamondRed = diamondRed;
        this.isMusic = isMusic;
        this.isSound = isSound;
        this.speed = speed;
        this.items = items;
        this.types = types;
        this.equipments = equipments;
        this.gift = gift;
        this.day = day;
    }
}
public class DataPlayer : MonoBehaviour
{
    public string name;
    public int idPlayer;
    public int lv;
    public int gold;
    public int diamondPurple;
    public int diamondRed;
    public bool isMusic;
    public bool isSound;
    public float speed;
    public int[,] items;
    public int[,] types;
    public int[] equipments;
    public int gift;
    public string day;
    public DataGame ReturnClass()
    {
        return new DataGame(name, idPlayer, lv, gold, diamondPurple, diamondRed, isMusic, isSound, speed, items, types, equipments, gift, day);
    }
    public void setDataGame(DataGame dataGame)
    {
        name = dataGame.name;
        idPlayer = dataGame.idPlayer;
        lv = dataGame.lv;
        gold = dataGame.gold;
        diamondPurple = dataGame.diamondPurple;
        diamondRed = dataGame.diamondRed;
        isMusic = dataGame.isMusic;
        isSound = dataGame.isSound;
        speed = dataGame.speed;
        items = dataGame.items;
        types = dataGame.types;
        equipments = dataGame.equipments;
        gift = dataGame.gift;
        day = dataGame.day;
    }
}
