using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grow_testing_debug : MonoBehaviour
{
    public List<Renderer> growRenderers; // 改为 Renderer 类型以包括 MeshRenderer 和 SkinnedMeshRenderer
    public float timeToGrow = 5;
    public float refreshRate = 0.05f;
    [Range(0,1)]
    public float minGrow = 0f;
    [Range(0,1)]
    public float maxGrow = 1f;

    private List<Material> growMaterials = new List<Material>();
    private bool fullyGrown;

    public void showAnim()
    {
        // 获取所有 MeshRenderers 和 SkinnedMeshRenderers
        growRenderers = new List<Renderer>(GetComponentsInChildren<Renderer>());
        
        if(growRenderers.Count == 0) 
        {
            Debug.LogWarning("No Renderer found.");
            return;
        }

        for(int i = 0; i < growRenderers.Count; i++)
        {
            for (int j = 0; j < growRenderers[i].materials.Length; j++)
            {
                if(growRenderers[i].materials[j].HasProperty("_Grow"))
                {
                    growRenderers[i].materials[j].SetFloat("_Grow", minGrow);
                    growMaterials.Add(growRenderers[i].materials[j]);
                }
            }
        }

        StartCoroutine(StartGrowAfterDelay(1.0f));
    }

    IEnumerator StartGrowAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        for(int i = 0; i < growMaterials.Count; i++)
        {
            StartCoroutine(Grow(growMaterials[i]));
        }
    }

    IEnumerator Grow(Material mat)
    {
        float growValue = mat.GetFloat("_Grow");
        if(!fullyGrown)
        {
            while(growValue < maxGrow)
            {
                growValue += 1/(timeToGrow/refreshRate); 
                mat.SetFloat("_Grow", growValue);
                yield return new WaitForSeconds(refreshRate);
            }
        }
        else
        {
            while(growValue > minGrow)
            {
                growValue -= 1/(timeToGrow/refreshRate); 
                mat.SetFloat("_Grow", growValue);
                yield return new WaitForSeconds(refreshRate);
            }
        }

        fullyGrown = (growValue >= maxGrow);
    }
}
