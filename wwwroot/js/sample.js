﻿(function() {
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
     * CUSTOM RENDERERS:
     * *****************
     */
    sigma.canvas.edges.goo = function(e, s, t, ctx, settings) {
        var color = e.color,
            p = settings('prefix') || '',
            edgeColor = settings('edgeColor'),
            defaultNodeColor = settings('defaultNodeColor'),
            defaultEdgeColor = settings('defaultEdgeColor'),
            v,
            d,
            p1 = 5 / 6,
            p2 = 1 / 6;

        if (!color)
            switch (edgeColor) {
                case 'source':
                    color = s.color || defaultNodeColor;
                    break;
                case 'target':
                    color = t.color || defaultNodeColor;
                    break;
                default:
                    color = defaultEdgeColor;
                    break;
            }

        d = Math.sqrt(Math.pow(t[p + 'x'] - s[p + 'x'], 2) + Math.pow(t[p + 'y'] - s[p + 'y'], 2));
        v = {
            x: (t[p + 'x'] - s[p + 'x']) / d,
            y: (t[p + 'y'] - s[p + 'y']) / d
        };

        ctx.fillStyle = color;
        ctx.beginPath();
        ctx.moveTo(
          s[p + 'x'] + v.y * s[p + 'size'],
          s[p + 'y'] - v.x * s[p + 'size']
        );
        ctx.bezierCurveTo(
          s[p + 'x'] * p1 + t[p + 'x'] * p2 + v.y * e[p + 'size'],
          s[p + 'y'] * p1 + t[p + 'y'] * p2 - v.x * e[p + 'size'],
          t[p + 'x'] * p1 + s[p + 'x'] * p2 + v.y * e[p + 'size'],
          t[p + 'y'] * p1 + s[p + 'y'] * p2 - v.x * e[p + 'size'],
          t[p + 'x'] + v.y * t[p + 'size'],
          t[p + 'y'] - v.x * t[p + 'size']
        );
        ctx.lineTo(
          t[p + 'x'] - v.y * t[p + 'size'],
          t[p + 'y'] + v.x * t[p + 'size']
        );
        ctx.bezierCurveTo(
          t[p + 'x'] * p1 + s[p + 'x'] * p2 - v.y * e[p + 'size'],
          t[p + 'y'] * p1 + s[p + 'y'] * p2 + v.x * e[p + 'size'],
          s[p + 'x'] * p1 + t[p + 'x'] * p2 - v.y * e[p + 'size'],
          s[p + 'y'] * p1 + t[p + 'y'] * p2 + v.x * e[p + 'size'],
          s[p + 'x'] - v.y * s[p + 'size'],
          s[p + 'y'] + v.x * s[p + 'size']
        );
        ctx.closePath();
        ctx.fill();
    };

    sigma.canvas.nodes.goo = function(node, ctx, settings) {
        var prefix = settings('prefix') || '';

        ctx.fillStyle = node.color || settings('defaultNodeColor');
        ctx.beginPath();
        ctx.arc(
          node[prefix + 'x'],
          node[prefix + 'y'],
          node[prefix + 'size'],
          0,
          Math.PI * 2,
          true
        );
        ctx.closePath();
        ctx.fill();

        ctx.fillStyle = '#fff';
        ctx.beginPath();
        ctx.arc(
          node[prefix + 'x'],
          node[prefix + 'y'],
          node[prefix + 'size'] * 0.5,
          0,
          Math.PI * 2,
          true
        );
        ctx.closePath();
        ctx.fill();
    };




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
    disc = document.getElementById('disc');
    ground = document.getElementById('ground');
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
              type: 'goo'
          },
          {
              id: (++nId) + '',
              size: nodeRadius,
              x: 10,
              y: -100,
              dX: 0,
              dY: 0,
              type: 'goo'
          },
          {
              id: (++nId) + '',
              size: nodeRadius,
              x: 20,
              y: -80,
              dX: 0,
              dY: 0,
              type: 'goo'
          }
        ],
        edges: [
          {
              id: (++eId) + '',
              source: '1',
              target: '2',
              type: 'goo'
          },
          {
              id: (++eId) + '',
              source: '1',
              target: '3',
              type: 'goo'
          },
          {
              id: (++eId) + '',
              source: '2',
              target: '3',
              type: 'goo'
          }
        ]
    });

    function frame() {
        s.refresh();

        if (s.graph.nodes().length) {
            var w = dom.offsetWidth,
                h = dom.offsetHeight;

            // The "rescale" middleware modifies the position of the nodes, but we
            // need here the camera to deal with this. Here is the code:
            var xMin = Infinity,
                xMax = -Infinity,
                yMin = Infinity,
                yMax = -Infinity,
                margin = 50,
                scale;

            s.graph.nodes().forEach(function(n) {
                xMin = Math.min(n.x, xMin);
                xMax = Math.max(n.x, xMax);
                yMin = Math.min(n.y, yMin);
                yMax = Math.max(n.y, yMax);
            });

            xMax += margin;
            xMin -= margin;
            yMax += margin;
            yMin -= margin;

            scale = Math.min(
              w / Math.max(xMax - xMin, 1),
              h / Math.max(yMax - yMin, 1)
            );

            c.goTo({
                x: (xMin + xMax) / 2,
                y: (yMin + yMax) / 2,
                ratio: 1 / scale
            });

            ground.style.top =
              Math.max(h / 2 - Math.min((yMin + yMax) / 2 * scale, h), 0) + 'px';
            disc.style.borderRadius = radius * scale;
            disc.style.width = 2 * radius * scale;
            disc.style.height = 2 * radius * scale;
            disc.style.top = mouseY - radius * scale;
            disc.style.left = mouseX - radius * scale;
            disc.style.backgroundColor = spaceMode ? '#f99' : '#9cf';

        }

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
                type: 'goo'
            });

        neighbors.forEach(function(n) {
            if (!spaceMode)
                s.graph.addEdge({
                    id: (++eId) + '',
                    source: id,
                    target: n.id,
                    type: 'goo'
                });
            else
                s.graph.dropNode(n.id);
        });
    }, false);
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
        spaceMode = (e.which == 32) ? true : spaceMode;
    });
    document.addEventListener('keyup', function(e) {
        spaceMode = e.which == 32 ? false : spaceMode;
    });
})();
