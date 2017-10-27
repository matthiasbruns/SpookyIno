using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using UniRx;

namespace ARena 
{
	[RequireComponent(typeof(NavMeshAgent))]
	public class PlayerActor : Actor, IRequireMovementInput
	{

		public static Optional<PlayerActor> FindLocalPlayerActor(){
			var actors = GameObject.FindObjectsOfType<PlayerActor> ();
			foreach(PlayerActor actor in actors){
				if (actor.isLocalPlayer) {
					return new Optional<PlayerActor>(actor);
				}
			}
			return new Optional<PlayerActor>();
		}

		[Range(0f, 10f)]
		public float walkSpeed = 5f;
		public GameObject body;

		private DestinationIndicator destinationIndicator;
		private NavMeshAgent navMeshAgent;
		private NetworkIdentity networkIdentity;
		private NetworkTransform networkTransform;
		private PlayerColorComponent playerColorComponent;

		protected IMovementInputProxy movementComponent;
	
		private ReactiveProperty<Vector3> destination = new ReactiveProperty<Vector3>(Vector3.zero);

		public Vector3 Destination {
			get {
				return destination.Value;
			}
			set {
				destination.Value = value;
			}
		}
        public IMovementInputProxy MovementInputProxy { get{return movementComponent;} set{movementComponent = value;} }

        void Awake()
		{
			gameObject.AddComponent<PlayerInjector> ();

			navMeshAgent = gameObject.GetComponent<NavMeshAgent> ();
			networkIdentity = gameObject.GetOrCreateComponent<NetworkIdentity> ();
			networkTransform = gameObject.GetOrCreateComponent<NetworkTransform> ();
			playerColorComponent = gameObject.GetOrCreateComponent<PlayerColorComponent> ();
			destinationIndicator = Singleton<DestinationIndicator>.Instance;			
		}

		void Start() {
			destination
				.Do(v => {
					destinationIndicator.gameObject.SetActive(v != Vector3.zero);
				})
				.Where(v => v != Vector3.zero)
				.Do(v => {
					destinationIndicator.transform.position = v;
				})
				.Subscribe(v => {
					navMeshAgent.destination = v;
				})
				.AddTo(this);


			movementComponent.DestinationObservable()
				.Where(v => v != Vector3.zero)
				.Subscribe(v => {
					destination.Value = v;
				}).AddTo(this);
		}

		void OnDrawGizmos(){
			if (destination.Value != Vector3.zero) {
				DrawPath (destination.Value);
			}
		}

		private void DrawPath(Vector3 destination){
			var path = navMeshAgent.path;
			var corners = path.corners;

			if (corners.Length <= 0) {
				return;
			}

			Debug.DrawLine (transform.position, path.corners [0], Color.red);

			for (int i = 0; i < path.corners.Length -1; i++){
				Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
			}

			Debug.DrawLine (path.corners [path.corners.Length -1], destination, Color.red);
		}
	}
}