using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum KeyBindingAction
{
    IDLE            = 0,
    MOVE_LEFT       = 1,
    MOVE_RIGHT      = 2,
    MOVE_UP         = 3,
    MOVE_DOWN       = 4,
    SHOOT           = 5,
}

public class InputManager : MonoBehaviour
{
    public InputData inputData;
    public SpaceShipController ship;


    private ICommand moveLeftCommand;
    private ICommand moveRightCommand;
    private ICommand moveUpCommand;
    private ICommand moveDownCommand;
    private ICommand stayCommand;
    private ICommand shootCommand;

    private void Awake()
    {
        moveLeftCommand = new MoveLeftCommand();
        moveRightCommand = new MoveRightCommand();
        moveUpCommand = new MoveUpCommand();
        moveDownCommand = new MoveDownCommand();
        shootCommand = new ShootCommand();
        stayCommand = new StayCommand();
    }
    private bool GetKey(KeyBindingAction action)
    {
        KeyCode key = inputData.inputDict[action];
        return Input.GetKey(key);
    }

    private void Update()
    {
        //Main Control Input
        if (ship.currentStatus == ShipStatus.NORMAL || ship.currentStatus == ShipStatus.INVINCIBLE)
        {
            if (GetKey(KeyBindingAction.SHOOT))
            {
                shootCommand.Execute(ship);
            }
            if (GetKey(KeyBindingAction.MOVE_LEFT))
            {
                moveLeftCommand.Execute(ship);
            }
            else if (GetKey(KeyBindingAction.MOVE_RIGHT))
            {
                moveRightCommand.Execute(ship);
            }
            else if (GetKey(KeyBindingAction.MOVE_DOWN))
            {
                moveDownCommand.Execute(ship);
            }
            else if (GetKey(KeyBindingAction.MOVE_UP))
            {
                moveUpCommand.Execute(ship);
            }
            else
            {
                //idle
                stayCommand.Execute(ship);
            }
        }
    }
}
