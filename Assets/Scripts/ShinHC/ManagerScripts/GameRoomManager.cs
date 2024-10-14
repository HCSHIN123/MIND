using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameRoomManager : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachine;

    [SerializeField] TransformationArea[] transformationAreas;
    [SerializeField] Transform playerSpawnPos;
    [SerializeField] Transform[] noxCheckPoints;
    [SerializeField] Transform[] LuxCheckPoints;
    [SerializeField] MovingFlat movingPlatForLux;
    [SerializeField] MovingFlat[] movingPlatForNox;
    [SerializeField] LightPath lightPath;
    [SerializeField] StartCredit credit;
    [SerializeField] OnOffTile[] onOffTiles;
    private GameObject basePlayer;
    private GameObject gamePlayer;

    private void Awake()
    {
        foreach (TransformationArea area in transformationAreas)
        {
            area.delTransformation = TransformationNewCharacter;
        }

        if (PhotonNetwork.IsMasterClient)   // 한명은 약간 옆에서 스폰되게끔 위치 수정
            playerSpawnPos.position = new Vector3(playerSpawnPos.position.x - 7f, playerSpawnPos.position.y, playerSpawnPos.position.z);

        basePlayer = PhotonNetwork.Instantiate("Player", new Vector3(playerSpawnPos.position.x, playerSpawnPos.position.y, playerSpawnPos.position.z), Quaternion.identity);
        cinemachine.Follow = basePlayer.GetComponentInChildren<CameraRoot>().transform;
        basePlayer.GetComponent<Player>().gameRoomManager = this;
    }

    public void TransformationNewCharacter(string _name, Transform _spawnPos)
    {
        credit.PlayerCredit();

        if (basePlayer == null || basePlayer.GetPhotonView().IsMine == false)
            return;
        Vector3 prevPos = basePlayer.transform.position;
        Quaternion prevRot = basePlayer.transform.rotation;
        PhotonNetwork.Destroy(basePlayer.GetPhotonView());
        basePlayer = null;

        gamePlayer = PhotonNetwork.Instantiate(_name, _spawnPos.position, prevRot);
        cinemachine.Follow = gamePlayer.GetComponentInChildren<CameraRoot>().transform;
        gamePlayer.GetComponent<Player>().gameRoomManager = this;
        

        if (_name == "Nox")
        {
            foreach(MovingFlat f in movingPlatForNox)
                f.player = gamePlayer.GetComponent<Player>();
            gamePlayer.GetComponent<Player>().Init(noxCheckPoints);
        }
        else
        {
            lightPath.lux = gamePlayer.GetComponent<Lux>();
            movingPlatForLux.player = gamePlayer.GetComponent<Player>();
            gamePlayer.GetComponent<Player>().Init(LuxCheckPoints);
            foreach(OnOffTile t in onOffTiles)
            {
                t.lux = lightPath.lux;
            }
        }
    }
}
