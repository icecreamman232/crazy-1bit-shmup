using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
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
    private void Update()
    {
        //Main Control Input
        if (ship.currentStatus == ShipStatus.NORMAL || ship.currentStatus == ShipStatus.INVINCIBLE)
        {
            if (Input.GetKey(KeyCode.G))
            {
                shootCommand.Execute(ship);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                moveLeftCommand.Execute(ship);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                moveRightCommand.Execute(ship);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                moveDownCommand.Execute(ship);
            }
            else if (Input.GetKey(KeyCode.UpArrow))
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
