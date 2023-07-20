using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBlendController : MonoBehaviour
{

    [SerializeField] SkinnedMeshRenderer _foodMesh;


    [SerializeField] bool blending1;
    [SerializeField] bool blending2;

    float blendValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {



        if (blending1)
        {
            if (blendValue < 100)
            {
                blendValue += Time.deltaTime*800;
                _foodMesh.SetBlendShapeWeight(0, blendValue);
            }
            else
            {
                blending1 = false;
               // SetBlend2();
            }

          
        }else


        if (blending2)
        {
            if (blendValue >0)
            {
                blendValue -= Time.deltaTime * 400;
                _foodMesh.SetBlendShapeWeight(0, blendValue);
            }
    

        }



    }

    public void SetBlend()
    {
        if (_foodMesh!=null)
        {
            blending1= true;
            blendValue = _foodMesh.GetBlendShapeWeight(0);
        }
    

    }

    public void SetBlend2()
    {
        if (_foodMesh != null)
        {
            blending2 = true;
            blendValue = _foodMesh.GetBlendShapeWeight(0);
        }
  

    }




    public void ResetBlend()
    {
        if (_foodMesh != null)
        {
            blending2 = false;
            blendValue = 0;
            _foodMesh.SetBlendShapeWeight(0, 0);
        }
 
    }

 


}
