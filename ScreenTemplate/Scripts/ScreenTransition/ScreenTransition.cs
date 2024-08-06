namespace AXitUnityTemplate.ScreenTemplate.Scripts.ScreenTransition
{
    using UnityEngine;
    using UnityEngine.Playables;
    using Cysharp.Threading.Tasks;
    using UnityEngine.EventSystems;
    using UITemplate.Scripts.Extension.Ulties;

    public class ScreenTransition : MonoBehaviour
    {
        [SerializeField] private PlayableDirector   intro;
        [SerializeField] private PlayableDirector   outro;
        [SerializeField] private bool               lockInput            = true;
        [SerializeField] private bool               playIntroFirstFrames = true;
        [SerializeField] private DirectorUpdateMode timeUpdateMode       = DirectorUpdateMode.UnscaledGameTime;

        private UniTaskCompletionSource animationTask;
        private EventSystem             eventSystem;

        private void Awake()
        {
            this.eventSystem = EventSystem.current;

            // Set intro to play first frames
            if (this.playIntroFirstFrames && this.intro != null)
            {
                this.intro.time = 0;
                this.intro.Evaluate();
                this.intro.Pause();
            }

            // Check and add event for intro and outro
            new[] { this.intro, this.outro }.ForEach(director =>
            {
                if (!director.playableAsset) return;
                director.timeUpdateMode =  this.timeUpdateMode;
                director.playOnAwake    =  false;
                director.stopped        += this.OnAnimationComplete;
            });
        }

        public UniTask PlayIntroAnimation() { return this.PlayAnimation(this.intro); }

        public UniTask PlayOutroAnimation() { return this.PlayAnimation(this.outro); }

        private UniTask PlayAnimation(PlayableDirector animationPlay)
        {
            if (!animationPlay.playableAsset || this.animationTask?.Task.Status == UniTaskStatus.Pending) return UniTask.CompletedTask;

            this.animationTask = new UniTaskCompletionSource();
            this.SetLookInput(false);
            animationPlay.Play();

            return this.animationTask.Task;
        }

        private void OnAnimationComplete(PlayableDirector director)
        {
            this.animationTask.TrySetResult();
            this.SetLookInput(true);
        }

        private void SetLookInput(bool value) { this.eventSystem.enabled = this.lockInput && this.eventSystem != null && value; }
    }
}