/**
 * This is a basic example on how to instantiate sigma. A random graph is
 * generated and stored in the "graph" variable, and then sigma is instantiated
 * directly with the graph.
 *
 * The simple instance of sigma is enough to make it render the graph on the on
 * the screen, since the graph is given directly to the constructor.
 */
var i,
    sig,
    nId = 0,
    eId = 0,

        radius = 50,

        spaceMode = false,
        wheelRatio = 1.1,

        nodeRadius = 10,
// Instantiate sigma:
sig = new sigma({
    container: 'graph-container',
    edgeColor: 'default',
    autoRescale: false,
    mouseEnabled: false,
    touchEnabled: false,
    nodesPowRatio: 1,
    edgesPowRatio: 1,
    defaultEdgeColor: '#333',
    defaultNodeColor: '#333',
});

$(function () {
    var serviceURL = '/network/graph';

    $.ajax({
        type: "GET",
        url: serviceURL,
        data: param = "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: successFunc,
        error: errorFunc
    });

    function successFunc(data, status) {
        sig.graph.clear();

        sig.graph.read(data);

        sig.refresh();
    }

    function errorFunc(ex) {
        alert('error: ' + ex);
    }
});


var dom = document.querySelector('#graph-container canvas:last-child');

/**
     * EVENTS BINDING:
     * ***************
     */
dom.addEventListener('click', function (e) {
    // Find neighbors:
    var x,
        y,
        p,
        id,
        neighbors;
     

    x = sigma.utils.getX(e) - dom.offsetWidth / 2;
    y = sigma.utils.getY(e) - dom.offsetHeight / 2;

    console.log(x, y);
    p = sig.camera.cameraPosition(x, y);
    x = p.x;
    y = p.y;

    console.log(x, y);

    neighbors = sig.graph.nodes().filter(function (n) {
        return (Math.sqrt(
          Math.pow(n.x - x, 2) +
          Math.pow(n.y - y, 2)
        ) - n.size) < radius;
    });

    if (!spaceMode)
        sig.graph.addNode({
            id: (id = (++nId) + ''),
            size: nodeRadius,
            x: x + Math.random() / 10,
            y: y + Math.random() / 10,
            dX: 0,
            dY: 0
        });

    neighbors.forEach(function (n) {
        if (!spaceMode)
            sig.graph.addEdge({
                id: (++eId) + '',
                source: id,
                target: n.id
            });
        else
            sig.graph.dropNode(n.id);
    });

    sig.refresh();
}, false);

dom.addEventListener('DOMMouseScroll', function (e) {
    radius *= sigma.utils.getDelta(e) < 0 ? 1 / wheelRatio : wheelRatio;
}, false);
dom.addEventListener('mousewheel', function (e) {
    radius *= sigma.utils.getDelta(e) < 0 ? 1 / wheelRatio : wheelRatio;
}, false);
document.addEventListener('keydown', function (e) {
    spaceMode = (e.which === 32) ? true : spaceMode;
});
document.addEventListener('keyup', function (e) {
    spaceMode = e.which === 32 ? false : spaceMode;
});