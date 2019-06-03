namespace VRTK.Examples
{
    using UnityEngine;
    using UnityEngine.UI;
    using VRTK.Controllables;

    public class ControllableReactorLeft : MonoBehaviour
    {
        public VRTK_BaseControllable controllable;
        public Text displayText;
        public string outputOnMax = "Maximum Reached";
        public string outputOnMin = "Minimum Reached";
        public Transform tf;
        private float speed;
        public float force;

        protected virtual void OnEnable()
        {
            controllable = (controllable == null ? GetComponent<VRTK_BaseControllable>() : controllable);
            controllable.ValueChanged += ValueChanged;
            controllable.MaxLimitReached += MaxLimitReached;
            controllable.MinLimitReached += MinLimitReached;
        }

        protected virtual void ValueChanged(object sender, ControllableEventArgs e)
        {
            if (displayText != null)
            {
                displayText.text = e.value.ToString("F1");
            }

            if (tf != null)
            {
                speed = e.value;
            }
        }

        protected virtual void MaxLimitReached(object sender, ControllableEventArgs e)
        {
            if (outputOnMax != "")
            {
                Debug.Log(outputOnMax);
            }
        }

        protected virtual void MinLimitReached(object sender, ControllableEventArgs e)
        {
            if (outputOnMin != "")
            {
                Debug.Log(outputOnMin);
            }
        }

        public void Update()
        {
            //rb.AddForce(0, 0, (accelerationValue * force));
            tf.position += speed * tf.forward * Time.deltaTime;
        }
    }
}