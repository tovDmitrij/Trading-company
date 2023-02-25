--Обновление курса валют (ежедневно)
DO $$
	DECLARE
		jid integer;
		scid integer;
	BEGIN
		INSERT INTO pgagent.pga_job(jobjclid, jobname, jobdesc, jobhostagent, jobenabled) 
		VALUES (1::integer, 'UpdateCources'::text, ''::text, ''::text, true) 
		RETURNING jobid INTO jid;

		INSERT INTO pgagent.pga_jobstep (jstjobid, jstname, jstenabled, jstkind, jstconnstr, jstdbname, jstonerror, jstcode, jstdesc) 
		VALUES (jid, 'step1'::text, true, 's'::character(1), ''::text, 'TradingCompany'::name, 'f'::character(1), 'call UpdateCources();'::text, ''::text);

		INSERT INTO pgagent.pga_schedule(jscjobid, jscname, jscdesc, jscenabled, jscstart, jscminutes, jschours, jscweekdays, jscmonthdays, jscmonths) 
		VALUES (jid, 'schedule1'::text, ''::text, true,
			'2023-02-26 19:44:00+03'::timestamp with time zone, 
			ARRAY[true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false]::boolean[],
			ARRAY[true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false]::boolean[],
			ARRAY[true,true,true,true,true,true,true]::boolean[],
			ARRAY[false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false]::boolean[],
			ARRAY[false,false,false,false,false,false,false,false,false,false,false,false]::boolean[]) 
		RETURNING jscid INTO scid;
	END 
$$;

--Обновление цен товаров (еженедельно)
DO $$
	DECLARE
		jid integer;
		scid integer;
	BEGIN
		INSERT INTO pgagent.pga_job(jobjclid, jobname, jobdesc, jobhostagent, jobenabled) 
		VALUES (1::integer, 'UpdatePrices'::text, ''::text, ''::text, true) 
		RETURNING jobid INTO jid;

		INSERT INTO pgagent.pga_jobstep (jstjobid, jstname, jstenabled, jstkind, jstconnstr, jstdbname, jstonerror, jstcode, jstdesc) 
		VALUES (jid, 'step1'::text, true, 's'::character(1), ''::text, 'TradingCompany'::name, 'f'::character(1), 'call UpdatePrices();'::text, ''::text);

		INSERT INTO pgagent.pga_schedule(jscjobid, jscname, jscdesc, jscenabled, jscstart, jscminutes, jschours, jscweekdays, jscmonthdays, jscmonths) 
		VALUES (jid, 'schedule1'::text, ''::text, true,
			'2023-02-26 19:50:00 +03:00'::timestamp with time zone, 
			ARRAY[false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false]::boolean[],
			ARRAY[false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false]::boolean[],
			ARRAY[true,false,false,false,false,false,false]::boolean[],
			ARRAY[false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false]::boolean[],
			ARRAY[false,false,false,false,false,false,false,false,false,false,false,false]::boolean[]) 
		RETURNING jscid INTO scid;
	END
$$;