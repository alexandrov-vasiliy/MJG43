using System;
using UnityEngine;
using Unity.Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineCamera vcMain;
    public CinemachineCamera vcFront;
    //public CinemachineCamera vcBack;

    private int _switcher = 0;

    void Start()
    {
        SetCamera(vcMain); // при старте активируем главную
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)&&_switcher<1)
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
            /*case -1:
                SetCamera(vcBack);
                break;*/
        }
        
    }

    void SetCamera(CinemachineCamera activeCam)
    {
        // Сброс всех приоритетов
        vcMain.Priority = 0;
        vcFront.Priority = 0;
        //vcBack.Priority = 0;

        // Устанавливаем активной
        activeCam.Priority = 10;
    }
}