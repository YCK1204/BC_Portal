using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    // Lazy 생성이 아니기 때문에 초기에 세팅이 필요
    // Awake 에서 추가할테니 다른 오브젝트에서는 Start 이후에
    private Player _player;
    public Player Player
    {
        get { return _player;  }
        set { _player = value; }
    }

    
}
