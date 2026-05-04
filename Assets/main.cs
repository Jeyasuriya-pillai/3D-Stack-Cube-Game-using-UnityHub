using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem; 
using UnityEngine.SceneManagement; 
public class main : MonoBehaviour
{
    public GameObject CurrentCube;
    public GameObject LastCube;
    public TextMeshProUGUI Text; 
    public int Level;
    public bool Done;
    void Start()
    {
        LastCube = CurrentCube;
        newBlock();
    }
    private void newBlock()
    {
        if (LastCube != null && Level > 0)
        {
            CurrentCube.transform.position = new Vector3(
                Mathf.Round(CurrentCube.transform.position.x),
                CurrentCube.transform.position.y,
                Mathf.Round(CurrentCube.transform.position.z)
            );
            float newX = LastCube.transform.localScale.x - Mathf.Abs(CurrentCube.transform.position.x - LastCube.transform.position.x);
            float newZ = LastCube.transform.localScale.z - Mathf.Abs(CurrentCube.transform.position.z - LastCube.transform.position.z);
            CurrentCube.transform.localScale = new Vector3(newX, LastCube.transform.localScale.y, newZ);
            CurrentCube.transform.position = Vector3.Lerp(CurrentCube.transform.position, LastCube.transform.position, 0.5f) + Vector3.up * 5f;
            if(CurrentCube.transform.localScale.x <= 0.1f || CurrentCube.transform.localScale.z <= 0.1f)
            {
                Done = true;
                if (Text != null)
                {
                    Text.gameObject.SetActive(true);
                    Text.text = "Score: " + Level;
                }
                StartCoroutine(RestartAfterDelay()); 
                return;
            }
        }
        LastCube = CurrentCube; 
        CurrentCube = Instantiate(LastCube);
        CurrentCube.transform.position = LastCube.transform.position + Vector3.up * 10f;
        CurrentCube.name = "Level_" + Level;
        MeshRenderer renderer = CurrentCube.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            float hue = (Level / 30.0f) % 1f; 
            renderer.material.color = Color.HSVToRGB(hue, 0.7f, 1f);
        }
        Level++;
        if (Text != null)
            Text.text = "Score: " + Level;
        Camera.main.transform.position = CurrentCube.transform.position + new Vector3(100, 100, 100);
        Camera.main.transform.LookAt(CurrentCube.transform.position);
    }
    void Update()
    {
        if (Done) return;
        var time = Mathf.Abs(Time.realtimeSinceStartup % 2f - 1f);
        var pos1 = LastCube.transform.position + Vector3.up * 10f;
        Vector3 moveDir = (Level % 2 == 0) ? Vector3.left : Vector3.forward;
        var pos2 = pos1 + moveDir * 120;
        CurrentCube.transform.position = Vector3.Lerp(pos1, pos2, time);
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            newBlock();
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            newBlock();
    }
    IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}