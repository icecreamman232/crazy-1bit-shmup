using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftCommand : ICommand
{
    public void Execute(SpaceShipController ship)
    {
        ship.ShipMoveLeft();
    }
}
public class MoveRightCommand : ICommand
{
    public void Execute(SpaceShipController ship)
    {
        ship.ShipMoveRight();
    }
}
public class MoveUpCommand : ICommand
{
    public void Execute(SpaceShipController ship)
    {
        ship.ShipMoveUp();
    }
}
public class MoveDownCommand : ICommand
{
    public void Execute(SpaceShipController ship)
    {
        ship.ShipMoveDown();
    }
}
public class StayCommand : ICommand
{
    public void Execute(SpaceShipController ship)
    {
        ship.ShipStay();
    }
}
public class ShootCommand : ICommand
{
    public void Execute(SpaceShipController ship)
    {
        ship.Shoot();
    }
}

