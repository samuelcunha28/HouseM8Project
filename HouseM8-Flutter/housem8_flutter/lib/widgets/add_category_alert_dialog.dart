import 'package:enum_to_string/enum_to_string.dart';
import 'package:flutter/material.dart';
import 'package:housem8_flutter/enums/categories.dart';
import 'package:recase/recase.dart';

class AddCategoryAlertDialog extends StatefulWidget {
  @override
  _AddCategoryAlertDialogState createState() => _AddCategoryAlertDialogState();
}

class _AddCategoryAlertDialogState extends State<AddCategoryAlertDialog> {
  Categories dropdownValue = Categories.GARDENING;

  @override
  Widget build(BuildContext context) {
    return AlertDialog(
      title: Text(
        'Adicione uma categoria:',
        style: TextStyle(fontSize: 16.0, color: Color(0xFF2F4858)),
      ),

      //Dropdown com as categorias
      content: new Container(
        child: new DropdownButton<Categories>(
          value: dropdownValue,
          icon: Icon(Icons.arrow_drop_down),
          iconSize: 24,
          isExpanded: true,
          elevation: 16,
          style: TextStyle(
            color: Color(0xFF5B82AA),
          ),
          underline: Container(
            height: 2,
            color: Color(0xFF5B82AA),
          ),
          items: Categories.values.map((Categories category) {
            return DropdownMenuItem<Categories>(
              value: category,
              child: Text(
                  new ReCase(EnumToString.convertToString(category)).titleCase),
            );
          }).toList(),
          onChanged: (Categories newValue) {
            dropdownValue = newValue;
            setState(() {});
          },
        ),
      ),
      //Botões
      actions: <Widget>[
        Row(
          mainAxisAlignment: MainAxisAlignment.center,
          crossAxisAlignment: CrossAxisAlignment.center,
          children: <Widget>[
            Container(
              width: MediaQuery.of(context).size.width * 0.20,
              child: RaisedButton(
                child: new Text(
                  'Guardar',
                  style: TextStyle(
                    color: Colors.white,
                  ),
                ),
                color: Color(0xFF2F4858),
                shape: new RoundedRectangleBorder(
                  borderRadius: new BorderRadius.circular(30.0),
                ),
                onPressed: () {
                  Navigator.of(context)
                      .pop(EnumToString.convertToString(dropdownValue));
                },
              ),
            ),
            SizedBox(
              width: MediaQuery.of(context).size.width * 0.01,
            ),
            Container(
              width: MediaQuery.of(context).size.width * 0.20,
              child: RaisedButton(
                child: new Text(
                  'Cancelar',
                  style: TextStyle(color: Colors.white),
                ),
                color: Color(0xFF2F4858),
                shape: new RoundedRectangleBorder(
                  borderRadius: new BorderRadius.circular(30.0),
                ),
                onPressed: () {
                  Navigator.of(context).pop();
                },
              ),
            ),
            SizedBox(
              height: MediaQuery.of(context).size.height * 0.02,
            ),
          ],
        )
      ],
    );
  }
}
