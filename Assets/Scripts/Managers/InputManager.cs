using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum KeyBindingAction
{
    ANYKEY          = -1,
    IDLE            = 0,
    MOVE_LEFT       = 1,
    MOVE_RIGHT      = 2,
    MOVE_UP         = 3,
    MOVE_DOWN       = 4,
    SHOOT           = 5,
}

public class InputManager : MonoBehaviour
{

    public static InputManager Instance;

    public InputData inputData;
    public SpaceShipController ship;

    private bool isAvailable;
    public bool firstKeyPressed;

    private ICommand moveLeftCommand;
    private ICommand moveRightCommand;
    private ICommand moveUpCommand;
    private ICommand moveDownCommand;
    private ICommand stayCommand;
    private ICommand shootCommand;

    private void Awake()
    {
        Instance = this;
        firstKeyPressed = false;
        moveLeftCommand = new MoveLeftCommand();
        moveRightCommand = new MoveRightCommand();
        moveUpCommand = new MoveUpCommand();
        moveDownCommand = new MoveDownCommand();
        shootCommand = new ShootCommand();
        stayCommand = new StayCommand();
    }
    private bool GetKey(KeyBindingAction action)
    {
        if (!isAvailable) return false;
        KeyCode key = inputData.inputDict[action];
        return Input.GetKey(key);
    }
    public void EnableInput()
    {
        isAvailable = true;
    }
    public void DisableInput()
    {
        isAvailable = false;
    }
    private void Update()
    {
        //Main Control Input
        if (ship.currentStatus == ShipStatus.NORMAL || ship.currentStatus == ShipStatus.INVINCIBLE)
        {   
            if(!firstKeyPressed)
            {
                if(Input.anyKey)
                {
                    firstKeyPressed = true;
                }
            }
            stayCommand.Execute(ship);
            if (GetKey(KeyBindingAction.SHOOT))
            {
                shootCommand.Execute(ship);
            }
            if (GetKey(KeyBindingAction.MOVE_LEFT))
            {
                moveLeftCommand.Execute(ship);
                if (GetKey(KeyBindingAction.MOVE_UP))
                {
                    moveUpCommand.Execute(ship);
                }
                else if(GetKey(KeyBindingAction.MOVE_DOWN))
                {
                    moveDownCommand.Execute(ship);
                }
                return;
            }
            if (GetKey(KeyBindingAction.MOVE_RIGHT))
            {
                moveRightCommand.Execute(ship);
                if (GetKey(KeyBindingAction.MOVE_UP))
                {
                    moveUpCommand.Execute(ship);
                }
                else if (GetKey(KeyBindingAction.MOVE_DOWN))
                {
                    moveDownCommand.Execute(ship);
                }
                return;
            }
            if (GetKey(KeyBindingAction.MOVE_UP))
            {
                moveUpCommand.Execute(ship);
                if (GetKey(KeyBindingAction.MOVE_LEFT))
                {
                    moveLeftCommand.Execute(ship);
                }
                else if (GetKey(KeyBindingAction.MOVE_RIGHT))
                {
                    moveRightCommand.Execute(ship);
                }
                return;
            }
            if (GetKey(KeyBindingAction.MOVE_DOWN))
            {
                moveDownCommand.Execute(ship);
                if (GetKey(KeyBindingAction.MOVE_LEFT))
                {
                    moveLeftCommand.Execute(ship);
                }
                else if (GetKey(KeyBindingAction.MOVE_RIGHT))
                {
                    moveRightCommand.Execute(ship);
                }
                return;
            }
        }
    }
}
