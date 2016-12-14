(function() {
    'use strict';

    var i,
        s,
        o,
        N = 100,
        E = 500,
        C = 5,
        d = 0.5,
        cs = [],
        g = {
            nodes: [],
            edges: []
        };

    for (i = 0; i < C; i++)
        cs.push({
            id: i,
            nodes: [],
            color: '#' + (
              Math.floor(Math.random() * 16777215).toString(16) + '000000'
            ).substr(0, 6)
        });

    for (i = 0; i < N; i++) {
        o = cs[(Math.random() * C) | 0];
        g.nodes.push({
            id: 'n' + i,
            label: 'Node' + i,
            x: Math.cos(2 * i * Math.PI / N),
            y: Math.sin(2 * i * Math.PI / N),
            size: 0.5 + 4.5 * Math.random(),
            color: o.color
        });
        o.nodes.push('n' + i);
    }

    for (i = 0; i < E; i++) {
        if (Math.random() < 1 - d)
            g.edges.push({
                id: 'e' + i,
                size: 0.5,
                source: 'n' + ((Math.random() * N) | 0),
                target: 'n' + ((Math.random() * N) | 0)
            });
        else {
            o = cs[(Math.random() * C) | 0]
            g.edges.push({
                id: 'e' + i,
                size: 0.5,
                source: o.nodes[(Math.random() * o.nodes.length) | 0],
                target: o.nodes[(Math.random() * o.nodes.length) | 0]
            });
        }
    }

    s = new sigma({
        graph: g,
        settings: {
            skipErrors: true
        }
    });

    // Initialize cameras:
    s.addCamera('cam1'),
    s.addCamera('cam2');

    // Initialize the three renderers:
    s.addRenderer({
        container: document.getElementById('webgl'),
        type: 'webgl',
        camera: 'cam1',
        settings: {
            defaultLabelColor: '#fff'
        }
    });

    s.addRenderer({
        container: document.getElementById('canvas2'),
        type: 'canvas',
        camera: 'cam1',
        settings: {
            drawEdges: false
        }
    });

    s.addRenderer({
        container: document.getElementById('canvas1'),
        type: 'canvas',
        camera: 'cam2'
    });

    // Start the layout algorithm:
    // s.startForceAtlas2();
    s.refresh();


    $(function () {
        var serviceURL = '/network/graph';

        $.ajax({
            type: "GET",
            url: serviceURL,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: successFunc,
            error: errorFunc
        });

        function successFunc(data, status) {
            // s.graph.clear();

            s.graph.read(data);

            s.refresh();

            //s.startForceAtlas2();
        }

        function errorFunc(ex) {
            alert('error: ' + ex);
        }
    });



})();
