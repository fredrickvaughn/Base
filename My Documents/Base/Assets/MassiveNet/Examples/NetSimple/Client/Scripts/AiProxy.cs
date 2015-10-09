﻿// MIT License (MIT) - Copyright (c) 2014 jakevn - Please see included LICENSE file
using MassiveNet;
using UnityEngine;

namespace Massive.Examples.NetSimple {
    public class AiProxy : MonoBehaviour {

        private NetView view;

        void Awake() {
            view = GetComponent<NetView>();
            // Note: Always register OnReadInstantiateData delegate in Awake
            // OnReadInstantiateData is called immediately after a View is created, so registering
            // in Start instead of Awake means you might miss out on the instantiate data.
            view.OnReadInstantiateData += Instantiate;
        }

        void Instantiate(NetStream stream) {
            transform.position = stream.ReadVector3();
        }
    }
}
