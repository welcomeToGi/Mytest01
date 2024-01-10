using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class ProgramExtensions : MonoBehaviour
{
    public static ProgramExtensions instance;
    public static GameObject target;
    Coroutine cor;
    public static void Create()
    {
        if (target == null)
        {
            target = new GameObject("[ProgramExtensions]");
            target.AddComponent<ProgramExtensions>();
        }
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public Coroutine WaitForCompletion(float delay, UnityAction action)
    {
        cor = StartCoroutine(ExecutAction(delay, action));
        return cor;
    }
    IEnumerator ExecutAction(float delay, UnityAction action)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
       // Destroy(target);
    }
   public void  Stop()
    {
        StopAllCoroutines();
    }
}
