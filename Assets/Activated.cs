using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
[System.Serializable]
public class Data
{
    public int id;
    public int pid;
    public int trial;
    public int touched_ball;
    //Rotation, intraction number, Interaction Type
    public Vector3 position;
    public Quaternion quaternion;
    public int timestamp;


    public void printData()
    {
        Debug.Log("Level : " + id);
        Debug.Log("Position : " + position);
    }
}


public class Activated : MonoBehaviour
{
    public GameObject cube, quad, answer;

    public Color[] list_color = {Color.black, Color.blue, Color.cyan, Color.grey, Color.green, Color.magenta, Color.white, Color.yellow};



    string ObjectToJson(object obj)
    {
        return JsonUtility.ToJson(obj);
    }

    T JsonToOject<T>(string jsonData)
    {
        return JsonUtility.FromJson<T>(jsonData);
    }
    


    string message;
    string loadMessage = "Yeah working";
    string data;
    FileInfo f;


    public float[] list_angle = { 0,20,40,60,80,100,120,140,160,180,200,220,240,260,280,300,320,340 };

    //public float[] list_angle = { 0,  45, 90,  135, 180,  225,  270,  315, 360 };

    public float[] list_distance = { 3,5,7 };
    List<int> no_angle_overlap = new List<int>();

    void Start()
    {




        var list_answer = new List<int>{ };


        
        //var n = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        //var sb = new StringBuilder();
        //for (int i = 1; i < 11; i++)
        //{
        //    var c = Combinations<int>.GetCombinations(n, i);
        //    sb.Length = 0;
        //    foreach (var k in c)
        //    {
        //        sb.Append('[');
        //        foreach (int v in k)
        //        {
        //            sb.Append(v).Append(", ");
        //        }
        //        sb.Remove(sb.Length - 2, 2);
        //        sb.Append("], ");
        //    }
        //    sb.Remove(sb.Length - 2, 2);
        //    Debug.Log("Combinations of length " + i + ": " + c.Count + "\n" + sb.ToString());
        //}
        f = new FileInfo(Application.persistentDataPath + "/" + "myFile.txt");


        for (int k = 1; k <= 3; k++){
            answer = GameObject.Find("/MixedRealityPlayspace/Main Camera/Quad (" + k + ")");
            var Renderer = answer.GetComponent<Renderer>();
            int xcount = Random.Range(0, 7);//7개
            list_answer.Add(xcount);
            Renderer.material.SetColor("_Color", list_color[xcount]);
        }
        
        foreach (object o in list_answer)
        {
            Debug.Log("Answer List " + o);
        }

        for (int i = 1; i <= 9; i++)
        {
            int rand = Random.Range(0, 16);
            rand = noCollideAngle(rand);
            var pos = RandomCircle(new Vector3(0, 0, 0), 0.5f, list_angle[rand]);
            var x = Mathf.Cos(list_angle[i]) * 1; 
            var z = Mathf.Sqrt(1 - Mathf.Pow(x, 2));



            var rotation = new Quaternion(0, 0, 0, 0);
            rotation = new Quaternion(x, 0, z, 0);


            //정답
            if (i == 1)
            {
                cube = GameObject.Find("/SceneContent/Model_Platonic (" + i + ")");




                for (int j = 1; j < 7; j++)
                {
                    if (j < 4)
                    {
                        foreach (int o in list_answer)
                        {
                            quad = GameObject.Find("/SceneContent/Model_Platonic (" + i + ")/Quad (" + j + ")");
                            //컬러 리스트 잡기(7중1). 색깔 찾아서 바꾸기.
                            //quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                            var Renderer1 = quad.GetComponent<Renderer>();
                            Renderer1.material.SetColor("_Color", list_color[o]);
                            if (j != 3)
                            {

                                j++;
                            }

                        }

                    }
                    else
                    {

                        quad = GameObject.Find("/SceneContent/Model_Platonic (" + i + ")/Quad (" + j + ")");
                        //컬러 리스트 잡기(7중1). 색깔 찾아서 바꾸기.
                        //quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                        var Renderer2 = quad.GetComponent<Renderer>();
                        int xcount = Random.Range(0, 7);
                        Renderer2.material.SetColor("_Color", list_color[xcount]);


                    }
                }

            
            cube.SetActive(true);
            cube.transform.position = pos;
            cube.transform.rotation = Quaternion.Euler(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
            }

            //오답들
            else
            {

             //여기서 0-2 랜덤 -> 걔 빼고 랜덤으로 색 입히기
             int rand2 = Random.Range(0, 3);
             int except =  list_answer[rand2];
             cube = GameObject.Find("/SceneContent/Model_Platonic ("+i+")");
             Debug.Log("exception cube:"+i+"color: "+ rand2+" ec: "+ except);



                for (int j = 1; j < 7; j++) {


                quad = GameObject.Find("/SceneContent/Model_Platonic (" + i + ")/Quad ("+ j +")");
                //컬러 리스트 잡기(7중1). 색깔 찾아서 바꾸기.
                //quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                var Renderer = quad.GetComponent<Renderer>();
                int xcount = Random.Range(0, 7);
                    //될때까지 다시.
                    if (xcount == except) {
                        Debug.Log("exception");
                        xcount = noCollideColor(except, xcount);


                    }

                Debug.Log("xcount"+xcount);

                Renderer.material.SetColor("_Color", list_color[xcount]);
                }

            }
            cube.SetActive(true);
            cube.transform.position = pos;
            cube.transform.rotation = Quaternion.Euler(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
        }

        Vector3 RandomCircle(Vector3 center, float radius, float a)
        {
            Debug.Log(a);

            int rand = Random.Range(0, 3);
            radius = list_distance[rand];
            Debug.Log("rad"+radius);

            float ang = a;
            Vector3 pos;
            pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
            pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
            pos.y = 0.1f;
            return pos;
        }

        int noCollideAngle( int i) {

            if (no_angle_overlap.Contains(i)) {
                int rand = Random.Range(0, 16);           
                return noCollideAngle(rand);
            }
            else {
                no_angle_overlap.Add(i);
                return i;
            }
            
        }


        int noCollideColor(int excep,int i)
        {
            if (excep == i)
            {
                int rand = Random.Range(0, 7);
                return noCollideColor(excep, rand);
            }
            else
            {
                return i;
            }
        }

      



    }


    int p = 0;
    //void Update()
    void FixedUpdate()
    {

        var now = System.DateTime.Now.ToLocalTime();
        var span = (now - new System.DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
        int timestamp = (int)span.TotalSeconds;

        Vector3 newPos = GameObject.Find("MixedRealityPlayspace/Main Camera/Pivot").transform.TransformPoint(Vector3.zero);
        //PID	Timestamp	Trial	Touched Ball	Position	Rotation	Distance (Calc by Position)	Distance (Calc by Rotation)	Interaction Number	Interaction Type	Device	Head Tracking	scale?													
        p = p + 1;
        Data data = new Data();
        data.id = p;
        data.position = new Vector3(newPos.x, newPos.y, newPos.z);
        data.timestamp = timestamp;
        string str = JsonUtility.ToJson(data);

        Debug.Log("ToJson : " + str);

        Save(str);

    }

   

    void OnGUI()
    {
        var style = new GUIStyle(GUI.skin.button);
        style.normal.textColor = Color.white;
        style.fontSize = 30;
        style.fixedHeight = 200;
        style.fixedWidth = 100;

        GUILayout.BeginArea(new Rect(100, 100, 500, 500));
        GUILayout.Label(message + " " + data);
        if (GUILayout.Button("Next", style))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
        if (GUILayout.Button("정답", style))
        {

        }
        GUILayout.EndArea();
    }

    void Save(string msg)
    {
        StreamWriter w;
        if (!f.Exists)
        {
            w = f.CreateText();
            w = f.AppendText();
        }
        else
        {
            w = f.AppendText();            
        }
        w.WriteLine(msg);
        w.Close();
        Debug.Log("SAVE");
    }

}