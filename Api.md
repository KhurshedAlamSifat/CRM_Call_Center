#######################################################
                TODO
#######################################################
- Falta incluir facturaci�n electr�nica en el API
- Falta validar cada campo y su tipo en los request del API
- Falta evanluar las comisiones de los bancos dependiendo del tipo
  cr[edito] en el banco, gebc debe pedir tabla de eso
- Poner clientOrderID en [PYF_PaymentRequest]


#######################################################
                NOTES
#######################################################
- SE SABE QUE UN PAGO NO HA SIDO FACTURADO PORUQUE EXISTE SU REGISTRO EN LA TABLA
    "PAYMENT" Y EL CAMBPO NroComprobantes es NULL o es "" vacio. Esto se puede
    deber a que el WebService JAVA no se inici[o] bien en el server.


#######################################################
                DOCUMENTATION
#######################################################

# PaymentRequest

### Method
/api/payment/create

### Request

Header:
    Content-Type    application/json
    Auth-Key        [API-KEY]

Json Body (Con Billing parameters):
````
{
	"company":"0923021539001",
  "person":{
      "document":"0923021539",
      "documentType":"05",
      "name":"juan carlos",
      "surname":"m�ndez guevara",
      "email":"jmendez@accroachcode.com",
      "mobile":"093249049",
      "address":{
         "street":"La joya",
         "city":"Guayaquil",
         "country":"EC"
      }
   },
  "paymentRequest":{
      "orderId":"1024",
      "description":"Compra en l�nea JCMG",
      "items":{
				"item":[
					 {
            "sku":"PRO04",
            "name":"PC Desktop 4Ghz",
            "qty":1,
            "price":10.00,
            "tax":1.20,
						"discount":0.00,
						"total":1.20
         }
				]
			}        
      ,
      "amount":{
        "taxes":[
            {
               "kind":"Iva",
               "amount":1.20,
               "base":10.00
            }
         ],
				"currency":"USD",
      	"total":11.20
      }      
   },
	"billingParameters":{
		"establecimiento":"001",
		"ptoEmision":"002",
		"infoAdicional":[
			{
				"key":"email",
				"value":"jfaksdj@afsdf.com"
			},
			{
				"key":"direccion",
				"value":"Guayaquil, una calle y la que cruza"
			}
		],
		"formaPago":"01",
		"plazoDias":"30"
	},
	"returnUrl":"https://client.com/return/1006",
	"cancelUrl":"https://client.com/cancel/1006",
	"userAgent":"PyF Api/1"
}
````
Json Body (Sin Billing parameters):
````
{
	"company":"0923021539001",
  "person":{
      "document":"0923021539",
      "documentType":"05",
      "name":"juan carlos",
      "surname":"m�ndez guevara",
      "email":"jmendez@accroachcode.com",
      "mobile":"093249049",
      "address":{
         "street":"La joya",
         "city":"Guayaquil",
         "country":"EC"
      }
   },
  "paymentRequest":{
      "orderId":"1033",
      "description":"Compra en l�nea JCMG",
      "items":{
				"item":[
					 {
            "sku":"PRO04",
            "name":"PC Desktop 4Ghz",
            "qty":1,
            "price":10.00,
            "tax":1.20,
						"discount":0.00,
						"total":1.20
         }
				]
			}        
      ,
      "amount":{
        "taxes":[
            {
               "kind":"Iva",
               "amount":1.20,
               "base":10.00
            }
         ],
				"currency":"USD",
      	"total":11.20
      }      
   },
	"returnUrl":"https://client.com/return/1006",
	"cancelUrl":"https://client.com/cancel/1006",
	"userAgent":"PyF Api/1"
}
````


### Response
 ````
{
  "status": {
    "status": "success",
    "message": "payment request successfully created",
    "reason": "",
    "date": "2020-05-26T01:37:16-05:00"
  },
  "requestId": "1115",
  "processUrl": "https://vpos.accroachcode.com/sandbox/18LKRnkxgZcFxPCVp0jW5"
}
````






# Postproceso

[STATUS_VALUE]:
APPROVED|APPROVED_PARTIAL|PENDING|PENDING_VALIDATION|REJECTED|FAILED|REFUNDED

### Invocaci�n en cliente
PAYLOAD: 
    status=[STATUS_VALUE]&
    requestId=[PYF_PaymentRequest.Id]&
    refRequestId=[P2P_PaymentRequest.ResponseRequestId]&
    orderId=[CLIENT_ORDER_ID]&
    statusReason=[MENSAJE RELACIONADO CON EL STATUS]

CONSULTA:
    https://[LINK DE POSTPROCESO EN CLIENTE]?xmlreq=base64(PAYLOAD)







# GetRequestInformation

[STATUS_VALUE]:
APPROVED|APPROVED_PARTIAL|PENDING|PENDING_VALIDATION|REJECTED|FAILED|REFUNDED

### Request

/api/payment/[RequestID] //Este es el PYF_PaymentRequest.Id retornado en el createPaymentRequest
Header:
    Content-Type    application/json
    Auth-Key        [API-KEY]

Fe: /api/payment/1116


### Response
````
{
  "requestId": "1116",
  "refRequestId": "163681",
  "status": {
    "status": "APPROVED",
    "message": "Pago aprobado con �xito",
    "reason": "",
    "date": "2020-05-26T01:40:41-05:00"
  },
  "request": null,
  "payment": null,
  "subscription": null,
  "comprobante": "001-002-000000053",
  "details": {
    "authorization": "999999",
    "subtotal": 1.00,
    "total": 1.12,
    "tax1": 0.12,
    "tax2": 0,
    "tax3": 0,
    "interest": 0.00,
    "creditType": "00",
    "installments": 1,
    "paymentMethod": "diners",
    "paymentMethodName": "Diners",
    "isssuerName": "Diners Club",
    "lastDigits": "0008",
    "expiration": "1229"
  }
}
````






# Parametros

TIPOS DE DOCUMENTO
CODIGO_SRI	DESC_IDENTIFICACION     CODIGO_P2P
04	        RUC	                    RUC
05	        CEDULA	                CI
06	        PASAPORTE	            PPN
08	        IDENTIFICACION EXT.	    LIC

FORMAS DE PAGO
ID_DB	ID_FORMA_PAGO	DESCRIPCION
1	    01	            SIN UTILIZACION DEL SISTEMA FINANCIERO
2	    15	            COMPENSACI�N DE DEUDAS
3	    16	            TARJETA DE D�BITO
4	    17	            DINERO ELECTR�NICO
5	    18	            TARJETA PREPAGO
6	    19	            TARJETA DE CR�DITO
7	    20	            OTROS CON UTILIZACION DEL SISTEMA FINANCIERO
8	    21	            ENDOSO DE T�TULOS




# Ejemplos

## GetRequestInformation

### A�n no se ingresa al vpos en navegador:
````
{
  "requestId": "", //PYF_PaymentRequest.Id
  "refRequestId": "",  //P2P_PaymentRequest.ResponseRequestId (RequestId directo de P2P)
  "status": {
    "status": "failure",
    "message": "error 702",
    "reason": "El RequestId solicitado no existe. ",
    "date": "2020-05-20T03:24:42-05:00"
  },
  "request": null,
  "payment": null,
  "subscription": null
}
````
### Ya se invoc[o] el VPOS de p2p pero no se ha pagado:
````
{
  "requestId": "8", //PYF_PaymentRequest.Id
  "refRequestId": "162600", //P2P_PaymentRequest.ResponseRequestId (RequestId directo de P2P)
  "status": {
    "status": "PENDING",
    "message": "La petici�n se encuentra activa",
    "reason": "PC",
    "date": "2020-05-20T03:00:18-05:00"
  },
  "request": null,
  "payment": null,
  "subscription": null
}
````
### Ya se ha pagado la transaccion:
````
{
  "requestId": "8",  //PYF_PaymentRequest.Id
  "refRequestId": "162600", //P2P_PaymentRequest.ResponseRequestId (RequestId directo de P2P)
  "status": {
    "status": "APPROVED",
    "message": "error P&F: An error occurred while updating the entries. See the inner exception for details.",
    "reason": "00",
    "date": "2020-05-20T03:03:00-05:00"
  },
  "request": null,
  "payment": null,
  "subscription": null
}
````


# Facturación

## Methods

/api/billing/create
/api/billing/list
/api/billing/[nro_autorizacion]
/api/billing/getride/[nro_autorizacion]
/api/billing/getxml/[nro_autorizacion]