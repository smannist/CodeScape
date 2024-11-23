using UnityEngine;
using Assets.Entities;

/* 
Variables declared here can be accessed in any other file, and keep their values between scene changes.
You can use them like: "Globals.docs"
*/
public static class Globals
{
	//contains the data read from JSON
	public static FileData docs;
	
	//store info about which floor was entered when moving to FloorInterior
	public static Class enteredFloor;
	public static Color floorColor;
	public static Function enteredRoom;
}
