DROP FUNCTION IF EXISTS public.get_frame_services(
    _service_layer_id integer,
    x1 double precision,
    y1 double precision,
    x2 double precision,
    y2 double precision);

DROP TYPE IF EXISTS serviceVM;

CREATE TYPE serviceVM as (
  service_id integer,
  name character varying(255),
  input_asset_id integer,
  output_asset_id integer
);

CREATE OR REPLACE FUNCTION public.get_frame_services(
    _service_layer_id integer,
    x1 double precision DEFAULT '-2147483648'::integer,
    y1 double precision DEFAULT '-2147483648'::integer,
    x2 double precision DEFAULT 2147483647,
    y2 double precision DEFAULT 2147483647)
  RETURNS setof serviceVM  AS
'
select _.service_id, _.name,_.input_asset_id, _.output_asset_id from ntw_service as _
LEFT JOIN ntw_asset_position AS n1 ON n1.asset_id = _.input_asset_id
LEFT JOIN ntw_asset_position AS n2 ON n2.asset_id = _.output_asset_id
where n1.x between x1 and x2 and n1.y between y1 and y2
and n2.x between x1 and x2 and n2.y between y1 and y2
and _.service_layer_id = _service_layer_id
order by _.service_id;
'
  LANGUAGE sql VOLATILE
  COST 100;
ALTER FUNCTION public.get_frame_services(integer, double precision, double precision, double precision, double precision)
  OWNER TO nms5000;
COMMENT ON FUNCTION public.get_frame_services(integer, double precision, double precision, double precision, double precision) IS 'Getting services from the frame (x1,y1,x2,y2) and service layer with id service_layer_id';

select * from public.get_frame_services(1,400,400,800,800);

select * from public.get_frame_services(1)