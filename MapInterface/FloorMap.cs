/* Date: 1.3.2014, Time: 14:07 */
using System;
using System.Collections.Generic;

namespace MapInterface
{
public struct TILE_INFO
{
	private int _Tile;
	public int Tile{
		get{
			return _Tile-1;
		}
		set{
			_Tile = value+1;
		}
	}
   public SURFACE pTile;
   public int TileImg;
   public int tVari;
   
	public enum WallSides : int
	{
		NORTH = 1,
		SOUTH = 8,
		EAST = 16,
		WEST = 32,
		SE = 64,
		SW = 128,
		NE = 256,
		NW = 512
	}

	public WallSides WallInfo;
   public int Wall;
   public int wVari;

   /*public TILE_INFO()
   {
	  Tile = (-1);
   }*/
}




public class FLOOR_MAP
{
	public TILE_INFO[,] tileMap;
	public List<SURFACE> TileImgs = new List<SURFACE>();
  public void Clear()
  {
	TileImgs.Clear();
  }

 public FLOOR_MAP()
 {
 	tileMap = new TILE_INFO[257,257];
 }

}
}
