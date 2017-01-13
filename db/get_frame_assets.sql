DROP FUNCTION IF EXISTS public.get_frame_assets(
    _service_layer_id integer,
    x1 double precision,
    y1 double precision,
    x2 double precision,
    y2 double precision);

DROP TYPE IF EXISTS assetVM;

CREATE TYPE assetVM as (
  asset_id integer,
  name character varying(255),
  x double precision,
  y double precision
);

CREATE OR REPLACE FUNCTION public.get_frame_assets(
    _service_layer_id integer,
    x1 double precision DEFAULT '-2147483648'::integer,
    y1 double precision DEFAULT '-2147483648'::integer,
    x2 double precision DEFAULT 2147483647,
    y2 double precision DEFAULT 2147483647)
  RETURNS setof assetVM  AS
'
select distinct _a.asset_id, _a.name, n.x, n.y from ntw_asset as _a
inner join ntw_service AS _ ON _a.asset_id = _.input_asset_id OR _a.asset_id = _.output_asset_id
LEFT JOIN ntw_asset_position AS n1 ON n1.asset_id = _.input_asset_id
LEFT JOIN ntw_asset_position AS n2 ON n2.asset_id = _.output_asset_id
inner join ntw_asset_position AS n ON n.asset_id = _a.asset_id
where (n1.x between x1 and x2 and n1.y between y1 and y2
or n2.x between x1 and x2 and n2.y between y1 and y2)
and _.service_layer_id = _service_layer_id
'
  LANGUAGE sql VOLATILE
  COST 100;
ALTER FUNCTION public.get_frame_assets(integer, double precision, double precision, double precision, double precision)
  OWNER TO nms5000;
COMMENT ON FUNCTION public.get_frame_assets(integer, double precision, double precision, double precision, double precision) IS 'Getting assets from the frame (x1,y1,x2,y2) and service layer with id service_layer_id';

select * from public.get_frame_assets(1,400,400,500,500);
select * from public.get_frame_assets(1);