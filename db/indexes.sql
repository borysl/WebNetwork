CREATE INDEX idx_x_coordinate on ntw_asset_position (x ASC)
CREATE INDEX idx_y_coordinate on ntw_asset_position (y ASC)
-- DROP INDEX idx_x_coordinate;
-- DROP INDEX idx_y_coordinate;
-- The following index is not that effective as the previous two or I don't know how to use it correctly
-- CREATE INDEX idx_coordinates on ntw_asset_position (x ASC, y ASC);
--DROP INDEX idx_coordinates;
