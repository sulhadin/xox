using System;
using UnityEngine;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour
{
    //public
    public GameObject[] Correctobjects;
    public GameObject WinPanel;
    public GameObject XWin;
    public GameObject OWin;
    public GameObject GameOverPanel;
    public GameObject XStone;
    public GameObject OStone;
    public Text You;
    public Text AI;
    public Stone StoneType;

    //private
    private GameObject _currentmatch;
    private bool Correctmatch = false;
    private Vector3 _startpos;
    private bool IsDragged = false;
    private static int YourPoint = 0;
    private static int AIPoint = 0;
    private bool IsSomeoneWin = false;
    private AudioSource StoneAudio;

    //private static
    private static readonly string[,] XOArray = new string[3, 3];
    private static bool _isMyTurn = true;
    private static readonly int[,] TheLastPlacedX = new int[1, 2];

    void Start()
    {
        StoneAudio = GetComponent<AudioSource>();

        _startpos = new Vector3(-5, -3, 0);//this.transform.position;
        //for (var i = 0; i < 3; i++)
        //{
        //    for (var j = 0; j < 3; j++)
        //    {
        //        _xArray[i, j] = "";
        //    }
        //}

    }

    private void LetTheComputerPlayz()
    {
        switch (GameManager.Level)
        {
            case GameManager.Levels.Easy:
                PlayzMode1();
                break;
            case GameManager.Levels.Normal:
                PlayzMode2();
                break;
            case GameManager.Levels.Hard:
                PlayzMode3();

                break;
        }
    }


    void OnMouseDown()
    {
    }

    void OnMouseDrag()
    {
        if (IsDragged) return;

        var newpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newpos.z = 0;
        this.transform.position = newpos;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        _currentmatch = col.gameObject;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        _currentmatch = null;
    }

    void OnMouseUp()
    {
        if (IsDragged) return;

        StoneAudio.Play();
        Correctmatch = false;
        if (_currentmatch != null)
        {
            foreach (var c in Correctobjects)
            {
                if (c.name != _currentmatch.name) continue;

                if (!IsPlaceAvailable(c.name))
                {
                    Goback();
                    return;
                }


                this.transform.position = c.transform.position;
                SetStoneInArray(c.name, Stone.X.ToString());
                Correctmatch = true;
                break;
            }
        }




        if (!Correctmatch)
        {
            Goback();
        }
        else
        {
            XWins();



            IsDragged = true;

            if (IsSomeoneWin) return;

            IsGameOver();

            Instantiate(XStone, _startpos, Quaternion.identity);

            LetTheComputerPlayz();


        }


    }

    public void Goback()
    {
        Correctmatch = false;
        this.transform.position = _startpos;
    }

    private void XWins()
    {

        if (WhoWins(Stone.X.ToString()))
        {
            //Debug.Log("GAME OVER FOR X");
            WinPanel.SetActive(true);
            IsSomeoneWin = true;
            YourPoint++;
            You.text = "You: " + YourPoint.ToString();
            OWin.gameObject.SetActive(false);
            XWin.gameObject.SetActive(true);
        }

    }

    private void OWins()
    {

        if (WhoWins(Stone.O.ToString()))
        {
            //Debug.Log("GAME OVER FOR O");

            WinPanel.SetActive(true);
            OWin.gameObject.SetActive(true);
            XWin.gameObject.SetActive(false);
            IsSomeoneWin = true;
            AIPoint++;
            AI.text = "AI : " + AIPoint.ToString();

        }
        else
        {
            IsGameOver();
        }


    }

    private bool IsPlaceAvailable(string name)
    {
        var indexX = Convert.ToInt32(name.Substring(0, 1)) - 1;
        var indexY = Convert.ToInt32(name.Substring(1, 1)) - 1;

        return !(XOArray[indexX, indexY] == Stone.X.ToString() || XOArray[indexX, indexY] == Stone.O.ToString());
    }

    private void SetStoneInArray(string name, string stonetype)
    {
        var indexX = Convert.ToInt32(name.Substring(0, 1)) - 1;
        var indexY = Convert.ToInt32(name.Substring(1, 1)) - 1;
        XOArray[indexX, indexY] = stonetype;
        if (stonetype == Stone.X.ToString())
        {
            TheLastPlacedX[0, 0] = indexX;
            TheLastPlacedX[0, 1] = indexY;
        }


    }

    private void PlayzMode1()
    {
        foreach (var obj in Correctobjects)
        {
            if (!IsPlaceAvailable(obj.name)) continue;

            Instantiate(OStone, obj.transform.position, Quaternion.identity);
            SetStoneInArray(obj.name, Stone.O.ToString());

            OWins();
            break;
        }
    }

    private void PlayzMode2()
    {
        var sldjkkgasdg = UnityEngine.Random.Range(0, 8);
        var sdfsdfStone = Correctobjects[sldjkkgasdg];

        if (!IsPlaceAvailable(sdfsdfStone.name))
        {
            PlayzMode2();
        }
        else
        {
            StoneAudio.Play();
            Instantiate(OStone, sdfsdfStone.transform.position, Quaternion.identity);
            SetStoneInArray(sdfsdfStone.name, Stone.O.ToString());
            OWins();
        }


    }

    private void PlayzMode3()
    {

        var newGoodPlace = FindGoodEmptyPlace();

        //if (newGoodPlace == null)
        //{
        //    Debug.Log("no good death");

        //    PlayzMode2();
        //    return;
        //}

        StoneAudio.Play();


        Debug.Log("found good place:" + newGoodPlace.transform.position);

        Instantiate(OStone, newGoodPlace.transform.position, Quaternion.identity);
        SetStoneInArray(newGoodPlace.name, Stone.O.ToString());
        OWins();
    }

    private GameObject FindGoodEmptyPlace()
    {
        GameObject InfinityStone = null;
        var isplacesempty = true;
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                if (XOArray[i, j] != "")
                {
                    isplacesempty = false;
                }
            }
        }

        if (isplacesempty)
        {
            return GetPlace(0, 0);
        }


        if (XOArray[1, 1] == "")
        {
            return GetPlace(1, 1);
        }


        if (XOArray[0, 2] == "")
        {
            return GetPlace(1, 1);
        }
        else if (XOArray[2, 0] == "")
        {
            return GetPlace(2, 0);
        }
        else if (XOArray[2, 2] == "")
        {
            return GetPlace(2, 2);
        }

        var x = TheLastPlacedX[0, 0];
        var y = TheLastPlacedX[0, 1];


        for (var i = x - 1; i < 3; i++)
        {

            for (var j = 0; j < 3; j++)
            {
                if (i == x && j == y) continue;

                if (i < 0 || i > 2 || j < 0 || j > 2) continue;

                Debug.Log("check if place is empty");

                if (XOArray[i, j] == "")
                {
                    return GetPlace(i, j);
                }
            }

        }




        Debug.Log("no action");
        PlayzMode2();

        return InfinityStone;


    }


    private GameObject GetPlace(int x, int y)
    {
        var index = 0;
        if (x == 0 && y == 0)
        {
            index = 0;
        }
        else if (x == 0 && y == 1)
        {
            index = 1;
        }
        else if (x == 0 && y == 2)
        {
            index = 2;
        }
        else if (x == 1 && y == 0)
        {
            index = 3;
        }
        else if (x == 1 && y == 1)
        {
            index = 4;
        }
        else if (x == 1 && y == 2)
        {
            index = 5;
        }
        else if (x == 2 && y == 0)
        {
            index = 6;
        }
        else if (x == 2 && y == 1)
        {
            index = 7;
        }
        else if (x == 2 && y == 2)
        {
            index = 8;
        }
        return Correctobjects[index];
    }


    private static bool WhoWins(string stonetype)
    {
        return (XOArray[0, 0] == stonetype && XOArray[0, 1] == stonetype && XOArray[0, 2] == stonetype) ||
               (XOArray[1, 0] == stonetype && XOArray[1, 1] == stonetype && XOArray[1, 2] == stonetype) ||
               (XOArray[2, 0] == stonetype && XOArray[2, 1] == stonetype && XOArray[2, 2] == stonetype) ||

               (XOArray[0, 0] == stonetype && XOArray[1, 0] == stonetype && XOArray[2, 0] == stonetype) ||
               (XOArray[0, 1] == stonetype && XOArray[1, 1] == stonetype && XOArray[2, 1] == stonetype) ||
               (XOArray[0, 2] == stonetype && XOArray[1, 2] == stonetype && XOArray[2, 2] == stonetype) ||

               (XOArray[0, 0] == stonetype && XOArray[1, 1] == stonetype && XOArray[2, 2] == stonetype) ||
               (XOArray[0, 2] == stonetype && XOArray[1, 1] == stonetype && XOArray[2, 0] == stonetype);
    }

    private void IsGameOver()
    {
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                if (!(XOArray[i, j] == Stone.X.ToString() || XOArray[i, j] == Stone.O.ToString()))
                {
                    return;
                }

            }
        }

        Debug.Log("game over");
        GameOverPanel.SetActive(true);

    }

    public void Replay()
    {


        WinPanel.SetActive(false);
        GameOverPanel.SetActive(false);

        Correctmatch = false;
        IsDragged = false;
        IsSomeoneWin = false;

        _isMyTurn = !_isMyTurn;

        var Stones = GameObject.FindGameObjectsWithTag("Stone");


        Debug.Log(Stones.Length);
        foreach (var item in Stones)
        {
            if (item.name.Contains("Clone"))
            {
                Destroy(item);
            }
            else
            {
                switch (item.name)
                {
                    case "XStone":
                        item.transform.position = _startpos;
                        break;
                    case "OStone":
                        item.transform.position = new Vector3(_startpos.x, _startpos.y, -15.39f);
                        break;
                }
            }

        }



        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                XOArray[i, j] = "";

            }
        }


        if (!_isMyTurn)
        {
            LetTheComputerPlayz();
        }
    }

    public enum Stone
    {
        X,
        O
    };
}
