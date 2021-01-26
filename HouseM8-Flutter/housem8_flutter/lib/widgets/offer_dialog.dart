import 'package:flutter/material.dart';

class OfferDialog extends StatelessWidget {
  final String title = "Realizar Oferta de Preço";
  final String text = "Indique abaixo o valor da sua oferta: ";
  final double initialPrice;

  const OfferDialog({Key key, this.initialPrice}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    double offerPrice;
    return AlertDialog(
      title: Text(title),
      content: Container(
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            Text(text),
            TextFormField(
              decoration: InputDecoration(
                labelText: 'Preço',
              ),
              initialValue: initialPrice.toString(),
              keyboardType: TextInputType.number,
              onChanged: (price) {
                offerPrice = double.parse(price);
              },
            )
          ],
        ),
      ),
      actions: [
        RaisedButton(
          child: new Text(
            'Enviar',
            style: TextStyle(color: Colors.white),
          ),
          color: Color(0xFF39A3ED),
          shape: new RoundedRectangleBorder(
            borderRadius: new BorderRadius.circular(30.0),
          ),
          onPressed: () {
            if (offerPrice != null) {
              Navigator.of(context).pop(offerPrice.toString());
            } else {
              Navigator.of(context).pop(initialPrice.toString());
            }
          },
        ),
      ],
    );
  }
}
