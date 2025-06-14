// ActivePack Library
// Copyright (C) BigBlit Assets Michal Kalinowski
// http://bigblit.fun
//

using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Assertions;
using UnityEngine.Playables;

namespace BigBlit.ShootingRange
{
    /// <summary>
    /// Animates GameObject that implements IValueable interface by using Playables and AnimationClips. 
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class ScriptableAnimator : MonoBehaviour
    {

        

        #region FIELDS AND PROPERTIES

        /// <summary>
        /// Allows to smooth out visual changes in positionable element position without actually smoothing the real value.
        /// </summary>
        [Tooltip("How smoothly the change of position should be animated (how long it's gonna take for animation to go from 0 to 1).")]
        [SerializeField, Range(0.0f, 1.0f)] float _animationSmoothing = 0.1f;

        /// <summary> AnimationClip that will be used for the animation. </summary>
        [Tooltip("AnimationClip that will be used for the animation.")]
        [SerializeField] AnimationClip _animationClip = null;

        /// <summary> Inverts the animation</summary>
        [Tooltip("Should animation be inverted that is go from 1 to 0.")]
        [SerializeField] bool _invert = false;


        [SerializeField] float _animClipRangeMin = 0.0f;

        [SerializeField] float _animClipRangeMax = 1.0f;

        public float TargetTime {
            //<summary>Gets the currently set animation time</summary>
            get => _targetTime;

            //<summary>Sets the animation time that will be smoothed out. </summary>
            set {
                if (_animationSmoothing > 0.001f) {
                    _targetTime = Mathf.Clamp(value, 0.0f, 0.999f);
                    _isDirty = true;
                }
                else {
                    Time = value;
                    _isDirty = false;
                }
            }
        }

        ///<summary>The currently set animation time.</summary>
        public float Time {   
            get => _time;
            private set {
                 
                _time = Mathf.Clamp(value, 0.0f, 1.0f);
                Assert.IsTrue(_playableGraph.IsValid());
                if (_animationClip == null)
                    return;

                float clipLength = _animationClip.length;
                float animationTime = (_invert ? Mathf.Lerp(_animClipRangeMin, _animClipRangeMax, 1.0f - _time)
                    : Mathf.Lerp(_animClipRangeMin, _animClipRangeMax, _time)) * clipLength;

                if (animationTime > clipLength)
                    animationTime = clipLength;
                else if (animationTime < 0.0f)
                    animationTime = 0.0f;

               _clipPlayable.SetTime(animationTime);
        
                _playableGraph.Evaluate();


            }
        }

        private float _targetTime;
        private float _time;
        private PlayableGraph _playableGraph;
        private AnimationClipPlayable _clipPlayable;
        private float _timeVel;  
        private bool _isDirty;
        private Vector3 _initialPosition;

        #endregion

        #region UNITY EVENTS
        void OnValidate() {
            _animClipRangeMin = Mathf.Clamp(_animClipRangeMin, 0.0f, 1.0f);
            _animClipRangeMax = Mathf.Clamp(_animClipRangeMax, 0.0f, 1.0f);

            _animationSmoothing = Mathf.Clamp(_animationSmoothing, 0.0f, 1.0f);

            if (Application.isPlaying && _playableGraph.IsValid()) {
                releaseAnimationGraph();
                setupAnimationGraph();
            }
        }

        void Awake() {
            //_target = GetComponent<IAnimable>();
            _initialPosition = transform.localPosition;
        }


        void Start() {

            setupAnimationGraph();
            //_target.timeChangeEvent += onTimeChanged;
            //Time = _target.Time;
        }


        void OnDestroy() {
            //_target.timeChangeEvent -= onTimeChanged;
            releaseAnimationGraph();
        } 

        void Update() {
            if (!_isDirty)
                return;

     

            if (Mathf.Abs(_time - _targetTime) < 0.001f) {
                Time = _targetTime;
                _isDirty = false;
            }
            else {
                Time = Mathf.SmoothDamp(_time, _targetTime, ref _timeVel, _animationSmoothing);
            }
        }

        #endregion

        #region INTERNAL FUNCTIONS

        private void setupAnimationGraph() {
            _playableGraph = PlayableGraph.Create();
            _playableGraph.SetTimeUpdateMode(DirectorUpdateMode.Manual);

            var playableOutput = AnimationPlayableOutput.Create(_playableGraph, "ScriptableAnimator", GetComponent<Animator>());

            _clipPlayable = AnimationClipPlayable.Create(_playableGraph, _animationClip);
            playableOutput.SetSourcePlayable(_clipPlayable);
        }

        private void releaseAnimationGraph() {
            _playableGraph.Destroy();
        }

        private void onTimeChanged(IAnimable animeable) {
            TargetTime = animeable.Time;
        }


        #endregion

    }
}