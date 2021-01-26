import 'package:flutter/material.dart';

class EmployerAppBar extends StatelessWidget with PreferredSizeWidget {
  @override
  final Size preferredSize;
  final String title;
  final bool icon;

  EmployerAppBar(
    this.title,
    this.icon, {
    Key key,
  })  : preferredSize = Size.fromHeight(50.0),
        super(key: key);

  @override
  Widget build(BuildContext context) {
    if (this.icon) {
      return AppBar(
        centerTitle: true,
        title: Text(this.title),
        backgroundColor: Color(0xFF93C901),
        actions: <Widget>[
          IconButton(
            icon: Icon(Icons.notifications),
            tooltip: 'Alertas',
            onPressed: () => print('Alertas'),
          ),
        ],
      );
    } else {
      return AppBar(
        centerTitle: true,
        title: Text(this.title),
        backgroundColor: Color(0xFF93C901),
      );
    }
  }
}
