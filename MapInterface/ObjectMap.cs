/* Date: 1.3.2014, Time: 14:05 */
using System;
using System.Collections.Generic;

namespace MapInterface
{
public class OBJECT
{
  public int callbackID;
  public int objID;
  public int imgID;
  public int X;
  public int Y;
  public int Height;
  public int Width;
  public bool ExtentCircle;
  public bool ExtentBox;
  public int Xlen;
  public int Ylen;
  public int Z;
  public int zSizeX;
  public int zSizeY;
  public int DoorFacing;
}

public class OBJECT_MAP
{
	public List<OBJECT> Objects = new List<OBJECT>();
	public List<SURFACE> ObjImages = new List<SURFACE>();

  public void Clear()
  {
	  Objects.Clear();
	  ObjImages.Clear();
  }
  public SURFACE FindImage(int ImgID)
  {
	  return ObjImages.Find(s => s.Image_ID == ImgID);
  }
  /*public void AddImage(int ObjID,int ImgID)
  {
  
  int numChildren;
  Surface surface;
  Surface name;
  int Image_ID;
  int Width;
  int Height;
  int OffsetX;
  int OffsetY;
  byte ImgType;

  }*/
 public OBJECT_MAP()
 {

 }

}
}
