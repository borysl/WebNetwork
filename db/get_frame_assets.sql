﻿CREATE TYPE assetVM as (
  asset_id integer,
  name character varying(255),
  x double precision,
  y double precision
)

CREATE OR REPLACE FUNCTION public.get_frame_assets(
    service_layer_id integer,
    x1 double precision DEFAULT '-100000'::integer,
    y1 double precision DEFAULT '-100000'::integer,
    x2 double precision DEFAULT 100000,
    y2 double precision DEFAULT 100000)
  RETURNS setof assetVM  AS
'
select _.asset_id, _.name, n.x, n.y from ntw_asset as _
LEFT JOIN ntw_asset_position AS n ON n.asset_id = _.asset_id
INNER JOIN sd_site AS s ON _.site_id = s.site_id
where n.x between x1 and x2 and n.y between y1 and y2
and n.service_layer_id = service_layer_id;
'
  LANGUAGE sql VOLATILE
  COST 100;
ALTER FUNCTION public.get_frame_assets(integer, double precision, double precision, double precision, double precision)
  OWNER TO nms5000;
COMMENT ON FUNCTION public.get_frame_assets(integer, double precision, double precision, double precision, double precision) IS 'Getting assets from the frame (x1,y1,x2,y2) and service layer with id service_layer_id';

select * from public.get_frame_assets(1,400,400,800,800)