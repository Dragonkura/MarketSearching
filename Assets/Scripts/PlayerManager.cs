using Amanotes.Utils;
using Element;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : SingletonMono<PlayerManager>
{
    public List<Player> allPlayers = new();
}
