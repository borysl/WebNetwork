;(function() {
    sigma.utils.pkg('sigma.canvas.nodes');
    sigma.canvas.nodes.square = function(node, context, settings) {
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
    }
})();

