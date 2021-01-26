import 'package:flutter/material.dart';

class RemoveAlertDialog extends StatelessWidget {
  final String title;
  final String message;

  RemoveAlertDialog(
    this.title,
    this.message, {
    Key key,
  });

  @override
  Widget build(BuildContext context) {
    return AlertDialog(
        title: Text(
          this.title,
          style: TextStyle(fontSize: 16.0, color: Colors.black),
        ),
        content: Text(
          this.message,
          style: TextStyle(fontSize: 14.0, color: Colors.black),
        ),
        actions: <Widget>[
          Row(
            mainAxisAlignment: MainAxisAlignment.center,
            crossAxisAlignment: CrossAxisAlignment.center,
            children: <Widget>[
              Container(
                width: MediaQuery.of(context).size.width * 0.20,
                child: RaisedButton(
                  child: new Text(
                    'Sim',
                    style: TextStyle(
                      color: Colors.white,
                    ),
                  ),
                  color: Colors.black,
                  shape: new RoundedRectangleBorder(
                    borderRadius: new BorderRadius.circular(30.0),
                  ),
                  onPressed: () {
                    Navigator.of(context).pop("Sim");
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
                    'Não',
                    style: TextStyle(color: Colors.white),
                  ),
                  color: Colors.black,
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
          ),
        ]);
  }
}
