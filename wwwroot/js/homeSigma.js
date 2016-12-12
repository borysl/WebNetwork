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
    N = 4,
    E = 6,
    g = {
        nodes: [],
        edges: []
    };
// Generate a random graph:
for (i = 0; i < N; i++)
    g.nodes.push({
        id: 'n' + i,
        label: 'Node ' + i,
        x: Math.random(),
        y: Math.random(),
        size: Math.random(),
        color: '#666'
    });
for (i = 0; i < E; i++)
    g.edges.push({
        id: 'e' + i,
        source: 'n' + (Math.random() * N | 0),
        target: 'n' + (Math.random() * N | 0),
        size: Math.random(),
        color: '#ccc'
    });
// Instantiate sigma:
sig = new sigma({
    graph: g,
    container: 'graph-container'
});

sig.graph.clear();
sig.graph.read({
    edges: [
        { "source": "262", "target": "586", "id": "6432" },
        { "source": "586", "target": "580", "id": "357" },
        { "source": "580", "target": "1015", "id": "4376" },
        { "source": "1015", "target": "262", "id": "5581" }],
    nodes: [
        { "label": "Sciences De La Terre", "x": 1412.2230224609375, "y": -2.055976390838623, "id": "262", "color": "rgb(255,204,102)", "size": 8.540210723876953 },
        { "label": "Champ", "x": -933.5524291992188, "y": 239.07545471191406, "id": "586", "color": "rgb(255,51,51)", "size": 4.0 },
        { "label": "Chaîne Trophique", "x": 1256.1710205078125, "y": -1671.3907470703125, "id": "580", "color": "rgb(153,255,0)", "size": 4.936610698699951 },
        { "label": "Kilometre Carre", "x": 173.3579559326172, "y": -567.8416137695312, "id": "1015", "color": "rgb(102,255,102)", "size": 5.103478908538818 }]
});