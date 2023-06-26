using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class AnimationTrackingLookAt : MonoBehaviour
{
    public Animator animator;
    public Transform targetAim;

    [System.Serializable]
    public class ItemClass
    {
        public GameObject element;
        public int triggerFrame = 0;
        public float motionSpeed = 10;
        public float blendAnim = 0.5f;
        public Vector3 aimVector = new Vector3(0,0,1);
        public Vector3 upVector = new Vector3(0,1,0);
        [HideInInspector] public bool isActive = false;
        [HideInInspector] public GameObject driver;
        [HideInInspector] public AimConstraint aimConstraint;
        [HideInInspector] public PositionConstraint positionConstraint;
        [HideInInspector] public ConstraintSource sourceConstraint;
        [HideInInspector] public ConstraintSource aimSourceConstraint;

        public void Setup(Transform aimSource)
        {
            driver = new GameObject("[driver] " + element.name);

            sourceConstraint = new ConstraintSource();
            sourceConstraint.sourceTransform = driver.transform;
            sourceConstraint.weight = 1;
            positionConstraint = (PositionConstraint)element.AddComponent(typeof(PositionConstraint));
            positionConstraint.constraintActive = false;
            //parentConstraint.translationAxis = Axis.X | Axis.Y;
            positionConstraint.AddSource(sourceConstraint);

            aimSourceConstraint = new ConstraintSource();
            aimSourceConstraint.sourceTransform = aimSource;
            aimSourceConstraint.weight = 1;
            aimConstraint = (AimConstraint)element.AddComponent(typeof(AimConstraint));
            aimConstraint.constraintActive = false;
            //aimConstraint.rotationAxis = Axis.Y;
            aimConstraint.aimVector = -Vector3.back;
            aimConstraint.AddSource(aimSourceConstraint);

            aimConstraint.aimVector = aimVector;
            aimConstraint.upVector = upVector;
        }
    }

    public List<ItemClass> itemList = new List<ItemClass>();

    void Update()
    {
        var clip = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
        var state = animator.GetCurrentAnimatorStateInfo(0);
        var frame = Mathf.Round(clip.length * (state.normalizedTime % 1) * clip.frameRate);

        if (frame <= 1) Reset();

        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].isActive) continue;

            if (frame > itemList[i].triggerFrame)
            {
                if(itemList[i].driver == null) itemList[i].Setup(targetAim);

                itemList[i].isActive = true;
                itemList[i].driver.transform.position = itemList[i].element.transform.position;
                itemList[i].driver.transform.rotation = itemList[i].element.transform.rotation;

                StartCoroutine(MotionRoutine(itemList[i]));

                itemList[i].positionConstraint.constraintActive = true;
                itemList[i].aimConstraint.constraintActive = true;
            }
        }
    }

    IEnumerator MotionRoutine(ItemClass item)
    {
        var blendValue = 0f;
        var targetAim = new Vector3(0,0,-1000);
        while (item.element.transform.position.z > targetAim.z)
        {
            targetAim = item.aimConstraint.GetSource(0).sourceTransform.transform.position;
            blendValue += Time.deltaTime * item.blendAnim;

            var value = Mathf.Lerp(0f, 1f, blendValue);
            item.aimConstraint.weight = value;
            item.positionConstraint.weight = value * 2;

            item.driver.transform.rotation = Quaternion.LookRotation(targetAim - item.driver.transform.position);
            item.driver.transform.Translate(0,0, item.motionSpeed * Time.deltaTime, Space.Self);
            yield return null;
        }
        ResetParent(item);
    }

    void Reset()
    {
        for (int i = 0; i < itemList.Count; i++) ResetItem(itemList[i]);
    }

    void ResetItem(ItemClass item)
    {
        item.isActive = false;
        ResetParent(item);
    }

    void ResetParent(ItemClass item)
    {
        if(item.positionConstraint) item.positionConstraint.constraintActive = false;
        if(item.aimConstraint) item.aimConstraint.constraintActive = false;
    }
}
