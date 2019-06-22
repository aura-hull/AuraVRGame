using System.Collections;
using System.Collections.Generic;
using OVRTouchSample;
using UnityEngine;
using VRTK;

public class HandAnimator : MonoBehaviour
{
    private static bool CLIP_ENDS = true;
    private static float THUMB_FACTOR = 0.5f;

    struct JointStructure
    {
        private Transform[] _joints;

        public Transform[] Joints
        {
            get { return _joints; }
        }

        public JointStructure(Transform parent)
        {
            List<Transform> joints = new List<Transform>();

            while (parent.childCount != 0)
            {
                joints.Add(parent);
                parent = parent.GetChild(0);
            }

            if (CLIP_ENDS) joints.RemoveAt(joints.Count - 1);

            _joints = joints.ToArray();
        }

        public void SetJointRotations(Quaternion[] rotations)
        {
            if (rotations.Length != _joints.Length) return;

            for (int i = 0; i < _joints.Length; i++)
            {
                _joints[i].localRotation = rotations[i];
            }
        }
    }

    struct HandStructure
    {
        private JointStructure _thumb;
        private JointStructure[] _fingers;

        public JointStructure Thumb
        {
            get { return _thumb; }
        }

        public JointStructure[] Fingers
        {
            get { return _fingers; }
        }

        public HandStructure(JointStructure thumb, params JointStructure[] fingers)
        {
            _thumb = thumb;
            _fingers = fingers;
        }

        public HandStructure(HandStructure other)
        {
            _thumb = other._thumb;
            _fingers = other._fingers;
        }

        public void SetPose(Pose pose)
        {
            _thumb.SetJointRotations(pose.GetThumbRotations());
            
            for (int i = 0; i < _fingers.Length; i++)
            {
                _fingers[i].SetJointRotations(pose.GetFingerRotations(i));
            }
        }
    }

    struct Pose
    {
        private Quaternion[] _thumbRotations;
        private Quaternion[][] _fingerRotations;

        public Quaternion[] GetThumbRotations()
        {
            return _thumbRotations;
        }

        public Quaternion[] GetFingerRotations(int index)
        {
            return _fingerRotations[index];
        }

        public Pose(HandStructure hand)
        {
            _thumbRotations = new Quaternion[hand.Thumb.Joints.Length];
            for (int i = 0; i < _thumbRotations.Length; i++)
            {
                _thumbRotations[i] = hand.Thumb.Joints[i].localRotation;
            }

            _fingerRotations = new Quaternion[hand.Fingers.Length][];
            for (int i = 0; i < _fingerRotations.Length; i++)
            {
                _fingerRotations[i] = new Quaternion[hand.Fingers[0].Joints.Length];

                for (int j = 0; j < _fingerRotations[i].Length; j++)
                {
                    _fingerRotations[i][j] = hand.Fingers[i].Joints[j].localRotation;
                }
            }
        }

        public void Rotate(float x, float y, float z)
        {
            float thumbX = x * THUMB_FACTOR;
            float thumbY = y * THUMB_FACTOR;
            float thumbZ = z * THUMB_FACTOR;

            for (int i = 0; i < _thumbRotations.Length; i++)
            {
                Vector3 euler = _thumbRotations[i].eulerAngles;
                euler = new Vector3(euler.x + thumbX, euler.y + thumbY, euler.z + thumbZ);
                _thumbRotations[i] = Quaternion.Euler(euler);
            }

            for (int i = 0; i < _fingerRotations.Length; i++)
            {
                for (int j = 0; j < _fingerRotations[i].Length; j++)
                {
                    Vector3 euler = _fingerRotations[i][j].eulerAngles;
                    euler = new Vector3(euler.x + x, euler.y + y, euler.z + z);
                    _fingerRotations[i][j] = Quaternion.Euler(euler);
                }
            }
        }
    }

    [SerializeField] private Transform thumb;
    [SerializeField] private Transform indexFinger;
    [SerializeField] private Transform middleFinger;
    [SerializeField] private Transform ringFinger;
    [SerializeField] private Transform pinkieFinger;
    [SerializeField] private VRTK_ControllerEvents controllerEvents;

    private HandStructure allBones;
    private Pose[] poses;
    private int currentPose = 0;

    void Start()
    {
        allBones = new HandStructure(
            new JointStructure(thumb),
            new JointStructure(indexFinger),
            new JointStructure(middleFinger),
            new JointStructure(ringFinger),
            new JointStructure(pinkieFinger));

        poses = new Pose[2];

        // Open hand
        poses[0] = new Pose(allBones);
        poses[0].Rotate(0.0f, 0.0f, -10.0f);

        // Closed hand
        poses[1] = new Pose(allBones);
        poses[1].Rotate(0.0f, 0.0f, -75.0f);

        if (controllerEvents != null)
        {
            controllerEvents.GripReleased += OpenHand;
            controllerEvents.GripPressed += CloseHand;
        }
    }

    private void OpenHand(object sender, ControllerInteractionEventArgs e)
    {
        if (currentPose == 0) return;
        allBones.SetPose(poses[0]);
        currentPose = 0;
    }

    private void CloseHand(object sender, ControllerInteractionEventArgs e)
    {
        if (currentPose == 1) return;
        allBones.SetPose(poses[1]);
        currentPose = 1;
    }
}
