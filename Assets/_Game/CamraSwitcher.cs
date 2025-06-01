using System;
using _Game;
using UnityEngine;
using Unity.Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineCamera vcMain;
    public CinemachineCamera vcFront;
    public CinemachineCamera vcBets;
    //public CinemachineCamera vcBack;


    private CinemachineCamera cachedCamera;

    private int _switcher = 0;

    private void Awake()
    {
        G.cameraSwitcher = this;
    }

    void Start()
    {
        SetCamera(vcMain); // при старте активируем главную
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space Pressed");
            SetCameraWithoutCache(vcMain);
        }
        else if(Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("Space Up");

            SetCamera(cachedCamera);
        }
      
        /*if (Input.GetKeyDown(KeyCode.W)&&_switcher<1)
        {
            _switcher++;
        }
        else if (Input.GetKeyDown(KeyCode.S)&&_switcher>-1)
        {
            _switcher--;
        }

        switch (_switcher)
        {
            case 0:
                SetCamera(vcMain);
                break;
            case 1:
                SetCamera(vcFront);
                break;
        }*/
        
    }

    public void SetCamera(CinemachineCamera activeCam)
    {
        SetCameraWithoutCache(activeCam);
        cachedCamera = activeCam;
    }

    private void SetCameraWithoutCache(CinemachineCamera cam)
    {
        // Сброс всех приоритетов
        vcMain.Priority = 0;
        vcFront.Priority = 0;
        //vcBack.Priority = 0;

        // Устанавливаем активной
        cam.Priority = 10;
    }
}