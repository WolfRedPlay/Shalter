


using System;
using System.Collections.Generic;

public enum CardType
{
    NONE = -1,
    BIOLOGY = 0,
    JOB = 1,
    HEALTH = 2,
    HOBBY = 3,
    BAGGAGE = 4,
    FACT = 5,
    SPECIAL = 6,
    CATASTROPHE = 7,
    BUNKER = 8
}

public enum Gender
{
    MALE,
    FEMALE,
    ANIMAL,
    ANDROID
}

public static class Times
{
    public const float SpeechTime = 60f;
    public const float ConversationTime = 300f;
    public const float VoteTime = 60f;
    public const float ExcuseTime = 60f;
    public const float CurrentPlayerTime = 1f;
    public const float ResultWindowTime = 5f;
    public const float UsedSpecilaCardTime = 3f;
    public const float BlendOutDuration = .5f;
}



public static class ListExtenssion
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1) 
        {
            n--;
            int k = new Random().Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

