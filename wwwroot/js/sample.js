(function() {
    'use strict';

    var s,
        c,
        dom,
        disc,
        ground,
        nId = 0,
        eId = 0,
        radius = 50,

        mouseX,
        mouseY,
        spaceMode = false,
        wheelRatio = 1.1,

        nodeRadius = 10,
        inertia = 0.8,
        springForce = 0.01,
        springLength = 50,
        maxDisplacement = 15,
        gravity = 1.5;



    /**
     * INITIALIZATION SCRIPT:
     * **********************
     */
    s = new sigma({
        renderer: {
            container: document.getElementById('graph-container'),
            type: 'canvas'
        },
        settings: {
            autoRescale: false,
            mouseEnabled: false,
            touchEnabled: false,
            nodesPowRatio: 1,
            edgesPowRatio: 1,
            defaultEdgeColor: '#333',
            defaultNodeColor: '#333',
            edgeColor: 'default'
        }
    });
    dom = document.querySelector('#graph-container canvas:last-child');
    c = s.camera;

    // Initialize graph:
    s.graph.read({
        nodes: [
          {
              id: (++nId) + '',
              size: nodeRadius,
              x: 0,
              y: -80,
              dX: 0,
              dY: 0,
          },
          {
              id: (++nId) + '',
              size: nodeRadius,
              x: 10,
              y: -100,
              dX: 0,
              dY: 0,
          },
          {
              id: (++nId) + '',
              size: nodeRadius,
              x: 20,
              y: -80,
              dX: 0,
              dY: 0,
          }
        ],
        edges: [
          {
              id: (++eId) + '',
              source: '1',
              target: '2',
          },
          {
              id: (++eId) + '',
              source: '1',
              target: '3',
          },
          {
              id: (++eId) + '',
              source: '2',
              target: '3',
          }
        ]
    });

    function frame() {
        s.refresh();

        requestAnimationFrame(frame);
    }

    frame();




    /**
     * EVENTS BINDING:
     * ***************
     */
    dom.addEventListener('click', function(e) {
        // Find neighbors:
        var x,
            y,
            p,
            id,
            neighbors;

        x = sigma.utils.getX(e) - dom.offsetWidth / 2;
        y = sigma.utils.getY(e) - dom.offsetHeight / 2;

        p = c.cameraPosition(x, y);
        x = p.x;
        y = p.y;

        neighbors = s.graph.nodes().filter(function(n) {
            return (Math.sqrt(
              Math.pow(n.x - x, 2) +
              Math.pow(n.y - y, 2)
            ) - n.size) < radius;
        });

        if (!spaceMode)
            s.graph.addNode({
                id: (id = (++nId) + ''),
                size: nodeRadius,
                x: x + Math.random() / 10,
                y: y + Math.random() / 10,
                dX: 0,
                dY: 0,
            });

        neighbors.forEach(function(n) {
            if (!spaceMode)
                s.graph.addEdge({
                    id: (++eId) + '',
                    source: id,
                    target: n.id,
                });
            else
                s.graph.dropNode(n.id);
        });
    }, false);

    dom = document.querySelector('#graph-container canvas:last-child');

    dom.addEventListener('mousemove', function(e) {
        mouseX = sigma.utils.getX(e);
        mouseY = sigma.utils.getY(e);
    }, false);
    dom.addEventListener('DOMMouseScroll', function(e) {
        radius *= sigma.utils.getDelta(e) < 0 ? 1 / wheelRatio : wheelRatio;
    }, false);
    dom.addEventListener('mousewheel', function(e) {
        radius *= sigma.utils.getDelta(e) < 0 ? 1 / wheelRatio : wheelRatio;
    }, false);
    document.addEventListener('keydown', function(e) {
        spaceMode = (e.which === 32) ? true : spaceMode;
    });
    document.addEventListener('keyup', function(e) {
        spaceMode = e.which === 32 ? false : spaceMode;
    });
})();
