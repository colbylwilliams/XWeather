//
//  BaseTvc.swift
//  XWeather
//
//  Created by Colby Williams on 9/13/16.
//  Copyright © 2016 Colby Williams. All rights reserved.
//

import Foundation
import UIKit

class BaseTvc : UITableViewController {
	
	let headerHeight: CGFloat = 280
	let footerHeight: CGFloat = 44

	var cellId: String? {
		get {
			fatalError("subclasses of BaseTvc must override cellId")
		}
	}
	
	
	override func viewDidLoad() {
		super.viewDidLoad()
		
		view.backgroundColor = UIColor.clear
		
		tableView.backgroundColor = UIColor.clear
	}
	
	
	override func scrollViewDidScroll(_ scrollView: UIScrollView) {
		
		maskCells(scrollView)
	}
	
	
	func maskCells(_ scrollView: UIScrollView) {
		
		for cell in tableView.visibleCells {
			
			let topHiddenHeight = scrollView.contentOffset.y + headerHeight - cell.frame.minY + scrollView.contentInset.top
			
			let bottomHiddenHeight = cell.frame.maxY - (scrollView.contentOffset.y + scrollView.frame.height - footerHeight)
			
			cell.setTransparencyMask(top: topHiddenHeight, bottom: bottomHiddenHeight)
		}
	}
	
	
	override func tableView(_ tableView: UITableView, heightForHeaderInSection section: Int) -> CGFloat {
		
		return headerHeight
	}
	
	
	override func tableView(_ tableView: UITableView, heightForFooterInSection section: Int) -> CGFloat {
		
		return footerHeight
	}
	
	
	override func tableView(_ tableView: UITableView, willDisplayHeaderView view: UIView, forSection section: Int) {
		
		if !tableView.visibleCells.isEmpty {
			
			maskCells(tableView)
		}
		
		if let header = view as? UITableViewHeaderFooterView {
			
			header.setBackgroundTransparent(labelColor: UIColor.white)
		}
	}
	
	
	override func tableView(_ tableView: UITableView, willDisplayFooterView view: UIView, forSection section: Int) {
		
		if let footer = view as? UITableViewHeaderFooterView {
			
			footer.setBackgroundTransparent(labelColor: UIColor.white)
		}
	}
	
	
	func dequeCell (_ tableView: UITableView, indexPath: IndexPath) -> UITableViewCell {
		
		return tableView.dequeueReusableCell(withIdentifier: cellId!, for: indexPath)
	}
	
	
	override var preferredStatusBarStyle: UIStatusBarStyle {
		
		return .lightContent
	}
}
