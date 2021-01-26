import 'package:flutter/material.dart';

class MateAppBar extends StatelessWidget with PreferredSizeWidget {
  @override
  final Size preferredSize;
  final String title;
  final bool icon;

  MateAppBar(
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
        backgroundColor: Color(0xFF39A3ED),
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
        backgroundColor: Color(0xFF39A3ED),
      );
    }
  }
}
