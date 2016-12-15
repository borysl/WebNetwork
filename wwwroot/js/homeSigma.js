/**
 * This is a basic example on how to instantiate sigma. A random graph is
 * generated and stored in the "graph" variable, and then sigma is instantiated
 * directly with the graph.
 *
 * The simple instance of sigma is enough to make it render the graph on the on
 * the screen, since the graph is given directly to the constructor.
 */

sigma.utils.pkg('sigma.canvas.nodes');
sigma.canvas.nodes.square = (function () {
    var _cache = {},
        _loading = {},
        _callbacks = {};

    // Return the renderer itself:
    var renderer = function (node, context, settings) {
        var prefix = settings('prefix') || '',
            size = node[prefix + 'size'],
            color = node.color || settings('defaultNodeColor'),
            roundRadius = size / 2,
            x = node[prefix + 'x'],
            y = node[prefix + 'y'];

        function rectangle(x, y, size, roundRadius, color) {
            context.beginPath();

            context.moveTo(x - size + roundRadius, y - size);
            context.lineTo(x + size - roundRadius, y - size);
            context.quadraticCurveTo(x + size, y - size, x + size, y - size + roundRadius);
            context.lineTo(x + size, y + size - roundRadius);
            context.quadraticCurveTo(x + size, y + size, x + size - roundRadius, y + size);
            context.lineTo(x - size + roundRadius, y + size);
            context.quadraticCurveTo(x - size, y + size, x - size, y + size - roundRadius);
            context.lineTo(x - size, y - size + roundRadius);
            context.quadraticCurveTo(x - size, y - size, x - size + roundRadius, y - size);

            context.fillStyle = color;
            context.fill();
        }

        rectangle(x, y, size, roundRadius, color);
        rectangle(x, y, size * .7, roundRadius * .7, "white");
    };

    // Let's add a public method to cache images, to make it possible to
    // preload images before the initial rendering:
    renderer.cache = function (url, callback) {
        if (callback)
            _callbacks[url] = callback;

        if (_loading[url])
            return;

        var img = new Image();

        img.onload = function () {
            _loading[url] = false;
            _cache[url] = img;

            if (_callbacks[url]) {
                _callbacks[url].call(this, img);
                delete _callbacks[url];
            }
        };

        _loading[url] = true;
        img.src = url;
    };

    return renderer;
})();

// Instantiate sigma:
var sig = new sigma({
    edgeColor: 'default',
    autoRescale: false,
    mouseEnabled: false,
    touchEnabled: false,
    nodesPowRatio: 1,
    edgesPowRatio: 1,
    defaultEdgeColor: '#333',
    defaultNodeColor: '#333',
    renderer: {
        container: document.getElementById('graph-container'),
        type: 'canvas'
    },
    settings: {
        animationsTime: 1000
    }
});

function loadGraph(suffix)
{
    var serviceURL = '/network/graph' + (suffix ? suffix : "");

    $.ajax({
        type: "GET",
        url: serviceURL,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: successFunc,
        error: errorFunc
    });

    function successFunc(data, status) {
        sig.graph.clear();

        sig.graph.read(data);

        sig.refresh(/*{ skipIndexation: true }*/);
    }

    function errorFunc(ex) {
        alert('error: ' + ex);
    }
}
$(function () {
    loadGraph();
});

var btnReload = document.getElementById("btnReload");
var txtFilter = document.getElementById("txtFilter");
var btnMagic = document.getElementById("btnMagic");

btnReload.onclick = function (e) {
    loadGraph(txtFilter.value);
};

btnMagic.onclick = function (e) {
    //sig.cameraPosition;
};


