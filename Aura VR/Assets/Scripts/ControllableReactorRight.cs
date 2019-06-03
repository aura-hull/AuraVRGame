namespace VRTK.Examples
{
    using UnityEngine;
    using UnityEngine.UI;
    using VRTK.Controllables;

    public class ControllableReactorRight : MonoBehaviour
    {
        public VRTK_BaseControllable controllable;
        public Text displayText;
        public string outputOnMax = "Maximum Reached";
        public string outputOnMin = "Minimum Reached";
        public Transform tf;
        private float rotationAmount;
        public float turnSpeed; //TurnSpeed for slower turning (realism)

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
                rotationAmount = e.value;
                //tf.rotation = Quaternion.Euler(0, e.value, 0);
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
        public void Update() //Calculate new direction and turn towards it
        {
            //Turning must be relative to where the boat is looking, so it keeps turning if the lever isn't at 0
            tf.Rotate(0, (rotationAmount * turnSpeed * Time.deltaTime), 0);
        }
    }
}