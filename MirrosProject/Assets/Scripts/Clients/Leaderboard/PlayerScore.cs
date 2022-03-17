using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace MirrorsProject.Clients
{
    public class PlayerScore : NetworkBehaviour
    {
        [SerializeField] TextMeshProUGUI nameText;
        [SerializeField] TextMeshProUGUI scoreText;

        public uint NetId { get; private set; }
        private Namer namer;
        private int score;
        public int GetScore() { return score; }
        public void SetPlayerName(string name)
        {
            nameText.text = name;
        }
        private void OnDestroy()
        {
            if (namer != null)
            {
                namer.Relay_OnNameUpdated -= Namer_Relay_OnNameUpdated;
            }
        }
        public void SetNetId(uint value)
        {
            NetId = value;
            if (NetworkIdentity.spawned.TryGetValue(NetId, out NetworkIdentity ni))
            {
                namer = ni.GetComponent<Namer>();
                namer.Relay_OnNameUpdated += Namer_Relay_OnNameUpdated;
            }
        }

        private void Namer_Relay_OnNameUpdated(string obj)
        {
            SetPlayerName(obj);
        }

        public void AddScore(int value)
        {
            score += value;
            scoreText.text = score.ToString();
        }
    }
}
