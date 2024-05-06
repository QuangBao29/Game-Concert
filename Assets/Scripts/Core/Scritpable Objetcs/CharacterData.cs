using System.Collections.Generic;
using UnityEngine;
using GameData;

[CreateAssetMenu(menuName = "Scriptable Object/Character Data")]
public class CharacterData : ScriptableObject
{
    public string ModelPath;

    public List<Character> ListCharacter;
}